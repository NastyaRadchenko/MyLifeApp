using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MyLifeClient.Models
{
    public class InputState
    {
        [Required(ErrorMessage = "Введите дату")]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }
        [Range(1, 500, ErrorMessage = "Вес не должен находиться вне диапазона 1..500")]
        [Required(ErrorMessage = "Введите вес")]

        public int MainWeigth { get; set; }

        [Range(0, 9, ErrorMessage = "Введите граммы, округлив до сотен")]
        public int PartialWeight { get; set; }
        public int Mood { get; set; }

        public InputState() { }
        public InputState(DateTime date, int mainWeigth, int partialWeight, int mood)
        {
            Date = date;
            MainWeigth = mainWeigth;
            PartialWeight = partialWeight;
            Mood = mood;
        }
    }
}
