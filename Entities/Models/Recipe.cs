using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models
{
    public class Recipe
    {
        [Column("RecipeId")]
        public Guid Id { get; set; }

        [ForeignKey(nameof(User))]
        public Guid UserId { get; set; }

        [Required(ErrorMessage = "Name is a required field.")]
        [MaxLength(20, ErrorMessage = "Maximum length for the Name is 20 characters.")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Ingredients is a required field.")]
        public string Ingredients { get; set; }
        public int TimeInMinutes { get; set; }
        [Required(ErrorMessage = "Complexity is a required field.")]
        public int Complexity { get; set; }
        public byte[] Picture { get; set; }
    }
}
