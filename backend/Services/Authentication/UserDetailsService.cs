using Microsoft.EntityFrameworkCore;
using Mapster;
using backend.Data;
using backend.DTOs.User;
using backend.Models;

namespace backend.Services.Authentication;

public class UserDetailsService : IUserDetailsService
{
    private readonly ZooManagementDbContext _context;

    public UserDetailsService(ZooManagementDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<UserDetailsDto>> GetAllUserDetailsAsync(UserDetailsQueryDto query)
    {
        var detailsQuery = _context.UserDetails.AsQueryable();

        if (!string.IsNullOrWhiteSpace(query.FirstName))
        {
            detailsQuery = detailsQuery.Where(ud => ud.FirstName.Contains(query.FirstName));
        }

        if (!string.IsNullOrWhiteSpace(query.LastName))
        {
            detailsQuery = detailsQuery.Where(ud => ud.LastName.Contains(query.LastName));
        }

        if (query.Gender.HasValue)
        {
            detailsQuery = detailsQuery.Where(ud => ud.Gender == query.Gender.Value);
        } 

        if (!string.IsNullOrWhiteSpace(query.Phone))
        {
            detailsQuery = detailsQuery.Where(ud => ud.Phone.Contains(query.Phone));
        }

        if (query.BirthDateFrom.HasValue)
        {
            detailsQuery = detailsQuery.Where(ud => ud.BirthDate >= query.BirthDateFrom.Value);
        }

        if (query.BirthDateTo.HasValue)
        {
            detailsQuery = detailsQuery.Where(ud => ud.BirthDate <= query.BirthDateTo.Value);
        }

        detailsQuery = query.SortBy?.ToLower() switch
        {
            "firstname" => query.SortDescending ? detailsQuery.OrderByDescending(ud => ud.FirstName) : detailsQuery.OrderBy(ud => ud.FirstName),
            "lastname" => query.SortDescending ? detailsQuery.OrderByDescending(ud => ud.LastName) : detailsQuery.OrderBy(ud => ud.LastName),
            "birthdate" => query.SortDescending ? detailsQuery.OrderByDescending(ud => ud.BirthDate) : detailsQuery.OrderBy(ud => ud.BirthDate),
            _ => query.SortDescending ? detailsQuery.OrderByDescending(ud => ud.UserId) : detailsQuery.OrderBy(ud => ud.UserId)
        };

        var userDetails = await detailsQuery
            .Skip((query.PageNumber - 1) * query.PageSize)
            .Take(query.PageSize)
            .ToListAsync();

        return userDetails.Adapt<List<UserDetailsDto>>();
    }

    public async Task<UserDetailsDto> GetUserDetailsByIdAsync(int userId)
    {
        var userDetails = await _context.UserDetails.FindAsync(userId);
        if (userDetails == null)
        {
            throw new KeyNotFoundException($"User details for user ID {userId} not found");
        }

        return userDetails.Adapt<UserDetailsDto>();
    }

    public async Task<IEnumerable<UserDetailsDto>> SearchByNameAsync(string firstName, string? lastName)
    {
        var query = _context.UserDetails.Where(ud => ud.FirstName.Contains(firstName));

        if (!string.IsNullOrWhiteSpace(lastName))
        {
            query = query.Where(ud => ud.LastName.Contains(lastName));
        }

        var userDetails = await query.ToListAsync();
        return userDetails.Adapt<List<UserDetailsDto>>();
    }

    public async Task<UserDetailsDto> GetUserDetailsByPhoneAsync(string phone)
    {
        var userDetails = await _context.UserDetails.FirstOrDefaultAsync(ud => ud.Phone == phone);
        if (userDetails == null)
        {
            throw new KeyNotFoundException($"User details with phone '{phone}' not found");
        }

        return userDetails.Adapt<UserDetailsDto>();
    }

    public async Task<UserDetailsDto> UpdateUserDetailsAsync(int userId, UpdateUserDetailsRequestDto request)
    {
        var userDetails = await _context.UserDetails.FindAsync(userId);
        if (userDetails == null)
        {
            throw new KeyNotFoundException($"User details for user ID {userId} not found");
        }

        if (!string.IsNullOrWhiteSpace(request.FirstName))
        {
            userDetails.FirstName = request.FirstName;
        }

        if (!string.IsNullOrWhiteSpace(request.LastName))
        {
            userDetails.LastName = request.LastName;
        }

        if (request.BirthDate.HasValue)
        {
            userDetails.BirthDate = request.BirthDate.Value;
        }

        if (request.Gender.HasValue)
        {
            userDetails.Gender = request.Gender.Value;
        }

        if (!string.IsNullOrWhiteSpace(request.Phone))
        {
            userDetails.Phone = request.Phone;
        }

        if (!string.IsNullOrWhiteSpace(request.HomeAddress))
        {
            userDetails.HomeAddress = request.HomeAddress;
        }

        if (!string.IsNullOrWhiteSpace(request.ImageUrl))
        {
            userDetails.ImageUrl = request.ImageUrl;
        }

        await _context.SaveChangesAsync();

        return userDetails.Adapt<UserDetailsDto>();
    }

    public async Task DeleteUserDetailsAsync(int userId)
    {
        var userDetails = await _context.UserDetails.FindAsync(userId);
        if (userDetails == null)
        {
            throw new KeyNotFoundException($"User details for user ID {userId} not found");
        }

        _context.UserDetails.Remove(userDetails);
        await _context.SaveChangesAsync();
    }
}