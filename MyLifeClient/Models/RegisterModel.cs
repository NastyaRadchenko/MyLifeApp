using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MyLifeClient.Models
{
    public class RegisterModel
    {
        [Required(ErrorMessage = "Введите имя пользователя")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Введите Email")]
        [EmailAddress(ErrorMessage = "Пожалуйста, введите корректный адрес")]
        [Remote(action: "ValidateRegisterEmail", controller: "Home")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Введите пароль")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required(ErrorMessage = "Повторите пароль")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Пароли должны совпадать")]
        public string ConfirmPassword { get; set; }
    }
}
