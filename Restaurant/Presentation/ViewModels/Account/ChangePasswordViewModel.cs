using System.ComponentModel.DataAnnotations;

namespace Restaurant.Presentation.ViewModels.Account;

public class ChangePasswordViewModel
{
    [Required]
    [DataType(DataType.Password)]
    [Display(Name = "Поточний пароль")]
    public string CurrentPassword { get; set; } = string.Empty;

    [Required]
    [DataType(DataType.Password)]
    [MinLength(6)]
    [Display(Name = "Новий пароль")]
    public string NewPassword { get; set; } = string.Empty;

    [Required]
    [DataType(DataType.Password)]
    [Compare(nameof(NewPassword))]
    [Display(Name = "Підтвердження нового пароля")]
    public string ConfirmNewPassword { get; set; } = string.Empty;
}
