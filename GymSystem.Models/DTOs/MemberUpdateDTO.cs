using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystem.Models.DTOs
{
    public class MemberUpdateDTO
    {
        [Required]
        public int Id { get; set; } 

        [Required]
        public string FullName { get; set; } = string.Empty;

        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;

        public string? Phone { get; set; }

        public DateTime? DateOfBirth { get; set; }
        public int? TrainerId { get; set; }
    }
}
