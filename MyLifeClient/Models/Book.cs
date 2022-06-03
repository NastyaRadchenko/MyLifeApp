using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MyLifeClient.Models
{
    public class Book
    {
        public Guid Id { get; set; }
        [Required(ErrorMessage = "Введите название")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Введите автора")]
        public string Author { get; set; }
        public string About { get; set; }
        public byte[] Picture { get; set; }
        public Guid UserId { get; set; }
        public Guid CategoryId { get; set; }
    }
}
