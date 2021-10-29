﻿using System.ComponentModel.DataAnnotations;

namespace EducateApp.ViewModels.Account
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Введите E - mail")]
        [Display(Name = "Email")]
        [DataType(DataType.EmailAddress)]   // тип элемента управления на станице 
        public string Email { get; set; }

        [Required(ErrorMessage = "Введите пароль")]
        [Display(Name = "Пароль")]
        [DataType(DataType.Password)]          // тип элемента управления на станице
        public string Password { get; set; }


        [Display(Name = "Запомнить?")]
        public bool RememberMe { get; set; }
        public string ReturnUrl { get; set; }
    }
}
