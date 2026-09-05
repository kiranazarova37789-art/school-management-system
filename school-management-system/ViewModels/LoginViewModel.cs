using System.ComponentModel.DataAnnotations;

namespace school_management_system.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Введите логин")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Введите пароль")]
        public string Password { get; set; }
    }
}
