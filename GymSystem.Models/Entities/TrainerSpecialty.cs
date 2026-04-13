using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystem.Models.Entities
{
    public class TrainerSpecialty
    {
        public int TrainerId { get; set; }
        public Trainer Trainer { get; set; }

        public int SpecialtyId { get; set; }
        public Specialty Specialty { get; set; }
    }
}
