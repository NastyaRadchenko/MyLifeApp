using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models
{
    public class DiaryEntry
    {
        [Column("EntryId")]
        public Guid Id { get; set; }
        [Required(ErrorMessage = "Date is a required field.")]
        public DateTime Date { get; set;}
        [Required(ErrorMessage = "Text is a required field.")]
        public string Text { get; set; }
        public byte[] Picture { get; set; }

        [ForeignKey(nameof(User))]
        public Guid UserId { get; set; }
    }
}
