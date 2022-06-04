using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models
{
    public class Stage
    {
        [Column("StageId")]
        public Guid Id { get; set; }
        [ForeignKey(nameof(Recipe))]
        public Guid RecipeId { get; set; }
        public int StageNumber { get; set; }
        public string Text { get; set; }
        public byte[] Picture { get; set; }
    }
}
