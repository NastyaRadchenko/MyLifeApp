using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MyLifeClient.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Введите email")]
        [EmailAddress(ErrorMessage = "Пожалуйста, введите корректный адрес")]
        public string Email { get; set; }
        [Remote(action: "ValidateLoginPassword", controller: "Home", AdditionalFields = nameof(Email))]
        [Required(ErrorMessage = "Введите пароль")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
