using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models
{
    public class User
    {
        [Column("UserId")]
        public Guid Id { get; set; }
        [Required(ErrorMessage = "User name is a required field.")]
        [MaxLength(20, ErrorMessage = "Maximum length for the Name is 20 characters.")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Email is a required field.")]
        [MaxLength(40, ErrorMessage = "Maximum length for the Email is 40 characters.")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Password hash is a required field.")]
        public string PasswordHash { get; set; }
        [ForeignKey(nameof(Role))]
        public Guid RoleId { get; set; }
    }
}
