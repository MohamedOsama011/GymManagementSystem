using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystem.Models.DTOs
{
    public class GymClassFormDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int TrainerId { get; set; }
        public int CategoryId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int Capacity { get; set; }
        public List<LookupItemDto> Trainers { get; set; } = new();
        public List<LookupItemDto> Categories { get; set; } = new();
    }
}
