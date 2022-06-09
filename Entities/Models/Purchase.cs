using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models
{
    public class Purchase
    {
        [Column("PurchaseId")]
        public Guid Id { get; set; }
        [ForeignKey(nameof(User))]
        public Guid UserId { get; set; }
        [ForeignKey(nameof(PurchaseCategory))]
        public Guid CategoryId { get; set; }
        [Required(ErrorMessage = "Name is a required field.")]
        [MaxLength(20, ErrorMessage = "Maximum length for the Name is 20 characters.")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Sum is a required field.")]
        public double Sum { get; set; }
        public DateTime Date { get; set; }
    }
}
