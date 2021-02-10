using System.ComponentModel.DataAnnotations;

namespace MovieCatalog.ViewModels
{
    public class RegisterViewModel
    {
        [Required]
        [Display(Name="Имя пользователя")]
        public string UserName { get; set; }
        
        [Required]
        [DataType(DataType.Password)]
        [Display(Name="Пароль")]
        public string Password { get; set; }

        [Required]
        [Compare("Password", ErrorMessage="Пароли не совпадают")]
        [DataType(DataType.Password)]
        [Display(Name="Подтверждение пароля")]
        public string ConfirmPassword { get; set; }
    }
}