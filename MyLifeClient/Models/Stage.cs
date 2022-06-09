using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MyLifeClient.Models
{
    public class Stage
    {
        public Guid Id { get; set; }
        public Guid RecipeId { get; set; }
        public int StageNumber { get; set; }
        [Required(ErrorMessage = "Введите описание этапа приготовление")]
        public string Text { get; set; }
        public byte[] Picture { get; set; }
    }
}
