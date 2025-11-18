using System.ComponentModel.DataAnnotations;
using backend.Enums;

namespace backend.DTOs.User;

public class ChangeUserRoleDto
{
    [Required(ErrorMessage = "New role ID is required")]
    [Range(1, 5, ErrorMessage = "Role ID must be greater than 0 and less than RoleName.Count")]
    public int NewRoleId { get; set; }
}