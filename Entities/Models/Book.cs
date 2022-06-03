using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models
{
    public class Book
    {
        [Column("EntryId")]
        public Guid Id { get; set; }
        [Required(ErrorMessage = "Name is a required field.")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Author is a required field.")]
        public string Author { get; set; }
        public string About { get; set; }
        public byte[] Picture { get; set; }

        [ForeignKey(nameof(User))]
        public Guid UserId { get; set; }

        [ForeignKey(nameof(BookCategory))]
        public Guid CategoryId { get; set; }
    }
}
