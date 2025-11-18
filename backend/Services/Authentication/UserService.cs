using Microsoft.EntityFrameworkCore;
using Mapster;
using backend.Data;
using backend.DTOs.Authentication;
using backend.DTOs.User;
using backend.Models;

namespace backend.Services.Authentication;

public class UserService : IUserService
{
    private readonly ZooManagementDbContext _context;

    public UserService(ZooManagementDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<UserResponseDto>> GetAllUsersAsync(UserQueryDto query)
    {
        var usersQuery = _context.UserAccounts
            .Include(u => u.CurrentRole)
            .Include(u => u.UserDetails)
            .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(query.Username))
        {
            usersQuery = usersQuery.Where(u => u.Username.Contains(query.Username));
        }

        if (!string.IsNullOrWhiteSpace(query.Email))
        {
            usersQuery = usersQuery.Where(u => u.Email.Contains(query.Email));
        }

        if (query.RoleId.HasValue)
        {
            usersQuery = usersQuery.Where(u => u.CurrentRoleId == query.RoleId.Value);
        }

        if (!string.IsNullOrWhiteSpace(query.RoleName))
        {
            usersQuery = usersQuery.Where(u => u.CurrentRole.Name.ToString().Contains(query.RoleName));
        }

        usersQuery = query.SortBy?.ToLower() switch
        {
            "username" => query.SortDescending ? usersQuery.OrderByDescending(u => u.Username) : usersQuery.OrderBy(u => u.Username),
            "email" => query.SortDescending ? usersQuery.OrderByDescending(u => u.Email) : usersQuery.OrderBy(u => u.Email),
            _ => query.SortDescending ? usersQuery.OrderByDescending(u => u.Id) : usersQuery.OrderBy(u => u.Id)
        };

        var users = await usersQuery
            .Skip((query.PageNumber - 1) * query.PageSize)
            .Take(query.PageSize)
            .ToListAsync();

        return users.Adapt<List<UserResponseDto>>();
    }

    public async Task<UserResponseDto> GetUserByIdAsync(int id)
    {
        var user = await _context.UserAccounts
            .Include(u => u.CurrentRole)
            .Include(u => u.UserDetails)
            .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
            .FirstOrDefaultAsync(u => u.Id == id);

        if (user == null)
        {
            throw new KeyNotFoundException($"User with ID {id} not found");
        }

        return user.Adapt<UserResponseDto>();
    }

    public async Task<UserResponseDto> GetUserByUsernameAsync(string username)
    {
        var user = await _context.UserAccounts
            .Include(u => u.CurrentRole)
            .Include(u => u.UserDetails)
            .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
            .FirstOrDefaultAsync(u => u.Username == username);

        if (user == null)
        {
            throw new KeyNotFoundException($"User with username '{username}' not found");
        }

        return user.Adapt<UserResponseDto>();
    }

    public async Task<UserResponseDto> GetUserByEmailAsync(string email)
    {
        var user = await _context.UserAccounts
            .Include(u => u.CurrentRole)
            .Include(u => u.UserDetails)
            .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
            .FirstOrDefaultAsync(u => u.Email == email);

        if (user == null)
        {
            throw new KeyNotFoundException($"User with email '{email}' not found");
        }

        return user.Adapt<UserResponseDto>();
    }

    public async Task<IEnumerable<UserResponseDto>> GetUsersByRoleAsync(int roleId)
    {
        var users = await _context.UserAccounts
            .Include(u => u.CurrentRole)
            .Include(u => u.UserDetails)
            .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
            .Where(u => u.CurrentRoleId == roleId)
            .ToListAsync();

        return users.Adapt<List<UserResponseDto>>();
    }

    public async Task<UserResponseDto> UpdateUserAsync(int id, UpdateUserRequestDto request)
    {
        var user = await _context.UserAccounts.FindAsync(id);
        if (user == null)
        {
            throw new KeyNotFoundException($"User with ID {id} not found");
        }

        if (!string.IsNullOrWhiteSpace(request.Username) && request.Username != user.Username)
        {
            if (await _context.UserAccounts.AnyAsync(u => u.Username == request.Username && u.Id != id))
            {
                throw new InvalidOperationException("Username already exists");
            }
            user.Username = request.Username;
        }

        if (!string.IsNullOrWhiteSpace(request.Email) && request.Email != user.Email)
        {
            if (await _context.UserAccounts.AnyAsync(u => u.Email == request.Email && u.Id != id))
            {
                throw new InvalidOperationException("Email already exists");
            }
            user.Email = request.Email;
        }

        await _context.SaveChangesAsync();

        return await GetUserByIdAsync(id);
    }

    public async Task DeleteUserAsync(int id)
    {
        var user = await _context.UserAccounts.FindAsync(id);
        if (user == null)
        {
            throw new KeyNotFoundException($"User with ID {id} not found");
        }

        _context.UserAccounts.Remove(user);
        await _context.SaveChangesAsync();
    }

    public async Task AssignRoleToUserAsync(int userId, int roleId)
    {
        var user = await _context.UserAccounts.FindAsync(userId);
        if (user == null)
        {
            throw new KeyNotFoundException($"User with ID {userId} not found");
        }

        var role = await _context.Roles.FindAsync(roleId);
        if (role == null)
        {
            throw new KeyNotFoundException($"Role with ID {roleId} not found");
        }

        var existingUserRole = await _context.UserRoles
            .FirstOrDefaultAsync(ur => ur.UserAccountId == userId && ur.RoleId == roleId);

        if (existingUserRole != null)
        {
            throw new InvalidOperationException("User already has this role");
        }

        var userRole = new UserRole
        {
            UserAccountId = userId,
            RoleId = roleId
        };

        _context.UserRoles.Add(userRole);
        await _context.SaveChangesAsync();
    }

    public async Task RemoveRoleFromUserAsync(int userId, int roleId)
    {
        var userRole = await _context.UserRoles
            .FirstOrDefaultAsync(ur => ur.UserAccountId == userId && ur.RoleId == roleId);

        if (userRole == null)
        {
            throw new KeyNotFoundException("User role assignment not found");
        }

        var user = await _context.UserAccounts.FindAsync(userId);
        if (user != null && user.CurrentRoleId == roleId)
        {
            throw new InvalidOperationException("Cannot remove the current active role. Change the current role first.");
        }

        _context.UserRoles.Remove(userRole);
        await _context.SaveChangesAsync();
    }

    public async Task<UserResponseDto> ChangeUserCurrentRoleAsync(int userId, int newRoleId)
    {
        var user = await _context.UserAccounts
            .Include(u => u.UserRoles)
            .FirstOrDefaultAsync(u => u.Id == userId);

        if (user == null)
        {
            throw new KeyNotFoundException($"User with ID {userId} not found");
        }

        var role = await _context.Roles.FindAsync(newRoleId);
        if (role == null)
        {
            throw new KeyNotFoundException($"Role with ID {newRoleId} not found");
        }

        var hasRole = user.UserRoles.Any(ur => ur.RoleId == newRoleId);
        if (!hasRole)
        {
            var userRole = new UserRole
            {
                UserAccountId = userId,
                RoleId = newRoleId
            };
            _context.UserRoles.Add(userRole);
        }

        user.CurrentRoleId = newRoleId;
        await _context.SaveChangesAsync();

        return await GetUserByIdAsync(userId);
    }
}