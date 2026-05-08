using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystem.Models.DTOs
{
    public class AttendanceRecordDto
    {
        public int Id { get; set; }
        public int MemberId { get; set; }
        public string MemberName { get; set; }
        public DateTime CheckInTime { get; set; }
        public DateTime? CheckOutTime { get; set; }
    }

    public class CheckInDto
    {
        public int MemberId { get; set; }
        public List<LookupItemDto> Members { get; set; } = new();
    }

    public class CheckInResultDto
    {
        public bool Success { get; set; }
        public string Message { get; set; }
    }

    public class AttendanceFilterDto
    {
        public int? MemberId { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
    }
}
