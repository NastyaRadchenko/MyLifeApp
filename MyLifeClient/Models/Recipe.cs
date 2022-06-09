using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MyLifeClient.Models
{
    public class Recipe
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }

        [Required(ErrorMessage = "Введите название блюда")]
        [MaxLength(20, ErrorMessage = "Название не должно превышать 20 символов")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Введите требуемые ингридиенты для рецепта")]
        public string Ingredients { get; set; }
        [Required(ErrorMessage = "Введите время приготовления блюда")]
        [Range(1, int.MaxValue, ErrorMessage = "Время не может быть отрицательным или нулевым")]
        public int TimeInMinutes { get; set; }
        public int Complexity { get; set; }
        public byte[] Picture { get; set; }
    }
}
