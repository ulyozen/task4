using Vision360.Context.Models;
using Vision360.DTOs;
using Vision360.Models;

namespace Vision360.Mappers;

public static class UserMapper
{
    public static User MapToUser(this AuthViewModel auth)
    {
        return new User { Name = auth.Name, UserName = auth.Email, Email = auth.Email };
    }

    public static List<AdminViewModel> MapToHomeViewModel(this IEnumerable<User> users)
    {
        return users.Select(u => u.MapToHomeViewModel()).ToList();
    }
    
    public static AdminViewModel MapToHomeViewModel(this User user)
    {
        return new AdminViewModel
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email,
            Status = user.IsActive,
            LastLogin = user.LastLoginTime
        };
    }
    
    public static PaginatedViewModel<AdminViewModel> MapToModel(this UserQueryDto dto, IList<User> users)
    {
        return new PaginatedViewModel<AdminViewModel>()
        {
            Data = users.MapToHomeViewModel().Skip((dto.Page - 1) * dto.Size).Take(dto.Size),
            CurrentPage = dto.Page,
            PageSize = dto.Size,
            TotalRecords = users.Count,
            SortColumn = dto.SortColumn,
            SortDirection = dto.SortDirection,
            SearchQuery = dto.Search
        };
    }
}