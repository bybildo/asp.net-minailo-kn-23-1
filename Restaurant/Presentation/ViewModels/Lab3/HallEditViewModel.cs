using Restaurant.Presentation.Validation;
using System.ComponentModel.DataAnnotations;

namespace Restaurant.Presentation.ViewModels.Lab3;

public class HallEditViewModel
{
    [Required]
    public Guid Id { get; set; }

    [Required(ErrorMessage = "Вкажіть назву залу.")]
    [StringLength(40, MinimumLength = 2, ErrorMessage = "Назва має бути від 2 до 40 символів.")]
    [Display(Name = "Назва залу")]
    [StartsWithUppercase]
    public string Name { get; set; } = string.Empty;

    [Range(1, 50, ErrorMessage = "Ширина має бути у межах 1..50.")]
    [Display(Name = "Ширина")]
    public int Width { get; set; }

    [Range(1, 50, ErrorMessage = "Довжина має бути у межах 1..50.")]
    [Display(Name = "Довжина")]
    public int Length { get; set; }
}
