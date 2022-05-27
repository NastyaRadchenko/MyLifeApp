using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MyLifeClient.Models
{
    public class DiaryEntry
    {
        public Guid Id { get; set; }
        [Required(ErrorMessage = "Выберите дату.")]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }
        [Required(ErrorMessage = "Невозможно сохранить пустую страницу")]
        public string Text { get; set; }
        public byte[] Picture { get; set; }
        public Guid UserId { get; set; }
    }
}
