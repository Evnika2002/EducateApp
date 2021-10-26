using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace EducateApp.Models
{
    public class User : IdentityUser
    {
        //Дополнительные поля для каждого пользователя
        //для преподователя могут понадобиться данные о ФИО

        //Сообщение об ошибке при валидации на стороне клиента
        [Required(ErrorMessage = "Введите фамилию")]

        //отображение Фамилии вместо LastName
        [Display(Name = "Фамилия")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Введите имя")]
        [Display(Name = "Имя")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Введите отчество")]
        [Display(Name = "Отчество")]
        public string Patronymic { get; set; }

        //навигационные свойства
    }
}
