using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystem.Models.DTOs
{
    public class TrainerDto
    {
        public string FullName { get; set; } = string.Empty;
        public string JobTitle { get; set; } = string.Empty;
        public string? PhotoPath { get; set; }
        public int Id { get; set; }
        public int MemberCount { get; set; }
        public int ClassCount { get; set; }
        public double WeeklyHours { get; set; }
        public int WeeklyHoursMax { get; set; } = 40;
        public double Rating { get; set; }
        public bool IsActive { get; set; }
        public IEnumerable<string> Specialties { get; set; } = new List<string>();
    }
}
