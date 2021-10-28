using System.ComponentModel.DataAnnotations;

namespace EducateApp.ViewModels
{
    //Т.е какие свойства (поля) "модель" нужно заполнить на определенной странице Html "представление"
    // на странице с регистрацией помимо E-mail  пароля нужно ввести информацию о преподователе - его ФИО
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Введите E - mail")]
        [Display(Name = "E - mail")]
        [DataType(DataType.EmailAddress)]   // тип элемента управления на станице 
        public string Email { get; set; }

        [Required(ErrorMessage = "Введите фамилию")]
        [Display(Name = "Фамилия")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Введите имя")]
        [Display(Name = "Имя")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Введите отчество")]
        [Display(Name = "Отчество")]
        public string Patronymic { get; set; }

        [Required(ErrorMessage = "Введите пароль")]
        [Display(Name = "Пароль")]
        [DataType(DataType.Password)]          // тип элемента управления на станице
        public string Password { get; set; }

        [Required(ErrorMessage = "Повторите ввод пароля")]
        [Compare("Password", ErrorMessage = "Пароли не совпадают")]     //механизм, который сверяет текущее значение PasswordConfirm, Password
        [DataType(DataType.Password)]          // тип элемента управления на станице
        [Display(Name = "Подтвердить пароль")]
        public string PasswordConfirm { get; set; }
    }
}
