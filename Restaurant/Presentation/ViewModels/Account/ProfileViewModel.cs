using System.ComponentModel.DataAnnotations;

namespace Restaurant.Presentation.ViewModels.Account;

public class ProfileViewModel
{
    [Required]
    [Display(Name = "Ім'я")]
    public string FullName { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;
}
