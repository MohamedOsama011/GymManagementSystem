using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystem.Models.Entities
{
    public class AttendanceRecord
    {
        public int Id { get; set; }
        public DateTime CheckInTime { get; set; }
        public DateTime? CheckOutTime { get; set; }

        public int MemberId { get; set; }
        public Member Member { get; set; }
    }
}
