using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models
{
    public class State
    {
        [Column("StateId")]
        public Guid Id { get; set; }
        [Required(ErrorMessage = "Date is a required field.")]
        public DateTime Date { get; set; }
        [Range(0, double.MaxValue, ErrorMessage = "Please enter valid weigthNumber")]
        [Required(ErrorMessage = "Weigth is a required field.")]
        public double Weigth { get; set; }
        [Range(1, 5, ErrorMessage = "Can only be between 1 .. 5")]
        [Required(ErrorMessage = "Mood is a required field.")]
        public int Mood { get; set; }

        [ForeignKey(nameof(User))]
        public Guid UserId { get; set; }
    }
}
