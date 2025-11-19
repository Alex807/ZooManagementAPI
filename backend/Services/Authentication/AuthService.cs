using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Mapster;
using backend.Data;
using backend.DTOs.Authentication;
using backend.DTOs.User;
using backend.Models;
using backend.Enums;

namespace backend.Services.Authentication;

public class AuthService : IAuthService
{
    private readonly ZooManagementDbContext _context;
    private readonly IConfiguration _configuration;

    public AuthService(ZooManagementDbContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }

    public async Task<AuthResponseDto> RegisterAsync(RegisterRequestDto request)
    {
        if (await _context.UserAccounts.AnyAsync(u => u.Username == request.Username))
        {
            throw new InvalidOperationException("Username already exists");
        }

        if (await _context.UserAccounts.AnyAsync(u => u.Email == request.Email))
        {
            throw new InvalidOperationException("Email already exists");
        }

        var roleId = request.RoleId ?? await GetVisitorRoleIdAsync(); //set the default role to Visitor if none provided
        var role = await _context.Roles.FindAsync(roleId);
        if (role == null)
        {
            throw new InvalidOperationException("Invalid role");
        }

        var passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);

        var userAccount = new UserAccount
        {
            Username = request.Username,
            Email = request.Email,
            PasswordHash = passwordHash,
            CurrentRoleId = roleId
        };

        _context.UserAccounts.Add(userAccount);
        await _context.SaveChangesAsync();

        var userDetails = new UserDetails
        {
            UserId = userAccount.Id,
            FirstName = request.FirstName,
            LastName = request.LastName, 
            BirthDate = request.BirthDate,
            Gender = request.Gender,
            Phone = request.Phone,
            HomeAddress = request.HomeAddress,
            ImageUrl = request.ImageUrl
        };

        _context.UserDetails.Add(userDetails);

        var userRole = new UserRole
        {
            UserAccountId = userAccount.Id,
            RoleId = roleId
        };

        _context.UserRoles.Add(userRole);
        await _context.SaveChangesAsync();

        var token = GenerateJwtToken(userAccount, role);
        var expiresAt = DateTime.UtcNow.AddHours(GetTokenExpirationHours());

        var userResponse = await GetUserResponseAsync(userAccount.Id);

        return new AuthResponseDto
        {
            Token = token,
            ExpiresAt = expiresAt,
            User = userResponse
        };
    }

    public async Task<AuthResponseDto> LoginAsync(LoginRequestDto request)
    {
        var userAccount = await _context.UserAccounts
            .Include(u => u.CurrentRole)
            .Include(u => u.UserDetails)
            .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
            .FirstOrDefaultAsync(u => u.Username == request.UsernameOrEmail || u.Email == request.UsernameOrEmail);

        if (userAccount == null || !BCrypt.Net.BCrypt.Verify(request.Password, userAccount.PasswordHash))
        {
            throw new UnauthorizedAccessException("Invalid credentials");
        }

        var token = GenerateJwtToken(userAccount, userAccount.CurrentRole);
        var expiresAt = DateTime.UtcNow.AddHours(GetTokenExpirationHours());

        var userResponse = await GetUserResponseAsync(userAccount.Id);

        return new AuthResponseDto
        {
            Token = token,
            ExpiresAt = expiresAt,
            User = userResponse
        };
    }

    public async Task ChangePasswordAsync(int userId, ChangePasswordRequestDto request)
    {
        var userAccount = await _context.UserAccounts.FindAsync(userId);
        if (userAccount == null)
        {
            throw new KeyNotFoundException("User not found");
        }

        if (!BCrypt.Net.BCrypt.Verify(request.CurrentPassword, userAccount.PasswordHash))
        {
            throw new UnauthorizedAccessException("Current password is incorrect");
        }

        userAccount.PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.NewPassword);
        await _context.SaveChangesAsync();
    }

    public async Task<AuthResponseDto> RefreshTokenAsync(int userId)
    {
        var userAccount = await _context.UserAccounts
            .Include(u => u.CurrentRole)
            .Include(u => u.UserDetails)
            .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
            .FirstOrDefaultAsync(u => u.Id == userId);

        if (userAccount == null)
        {
            throw new KeyNotFoundException("User not found");
        }

        var token = GenerateJwtToken(userAccount, userAccount.CurrentRole);
        var expiresAt = DateTime.UtcNow.AddHours(GetTokenExpirationHours());

        var userResponse = await GetUserResponseAsync(userAccount.Id);

        return new AuthResponseDto
        {
            Token = token,
            ExpiresAt = expiresAt,
            User = userResponse
        };
    }

    private string GenerateJwtToken(UserAccount user, Role role)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"] ?? throw new InvalidOperationException("JWT Key not configured")));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new Claim("UserId", user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, role.Name.ToString()),
            new Claim("CurrentRoleId", user.CurrentRoleId.ToString())
        };

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(GetTokenExpirationHours()),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private async Task<int> GetVisitorRoleIdAsync()
    {
        var visitorRole = await _context.Roles.FirstOrDefaultAsync(r => r.Name == RoleName.Visitor);
        if (visitorRole == null)
        {
            throw new InvalidOperationException("Visitor role not found in database");
        }
        return visitorRole.Id;
    }

    private async Task<UserResponseDto> GetUserResponseAsync(int userId)
    {
        var user = await _context.UserAccounts
            .Include(u => u.CurrentRole)
            .Include(u => u.UserDetails)
            .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
            .FirstOrDefaultAsync(u => u.Id == userId);

        if (user == null)
        {
            throw new KeyNotFoundException("User not found");
        }

        return user.Adapt<UserResponseDto>();
    }

    private int GetTokenExpirationHours()
    {
        return int.TryParse(_configuration["Jwt:ExpirationHours"], out var hours) ? hours : 24;
    }
}