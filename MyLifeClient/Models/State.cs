using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MyLifeClient.Models
{
    public class State
    {
        public Guid Id { get; set; }
        public DateTime Date { get; set; }
        public double Weigth { get; set; }
        public int Mood { get; set; }
        public Guid UserId { get; set; }

        public State() { }
        public State(Guid id, Guid userId, DateTime date, double weigth, int mood)
        {
            Id = id;
            UserId = userId;
            Date = date;
            Weigth = weigth;
            Mood = mood;
        }
    }
}
