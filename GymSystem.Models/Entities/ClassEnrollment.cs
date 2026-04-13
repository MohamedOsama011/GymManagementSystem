using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystem.Models.Entities
{
    public class ClassEnrollment
    {
        public int MemberId { get; set; }
        public Member Member { get; set; }

        public int ClassId { get; set; }
        public GymClass GymClass { get; set; }

        public DateTime EnrolledAt { get; set; } = DateTime.Now;
    }
}
