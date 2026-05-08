using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystem.Models.DTOs
{
    public class GymClassDetailsDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string TrainerName { get; set; }
        public string CategoryName { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int Capacity { get; set; }
        public int EnrolledCount { get; set; }
        public List<EnrolledMemberDto> EnrolledMembers { get; set; } = new();
        public List<LookupItemDto> AvailableMembers { get; set; } = new(); 
    }

    public class EnrolledMemberDto
    {
        public int MemberId { get; set; }
        public string MemberName { get; set; }
        public string MemberEmail { get; set; }
        public DateTime EnrolledAt { get; set; }
    }
}
