using GymSystem.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystem.BLL.Services.Interfaces
{
    public interface IGymClassService
    {
        Task<IEnumerable<GymClassListDto>> GetAllAsync(); 
        Task<GymClassFormDto> GetFormDataAsync(int? id = null); 
        Task CreateAsync(GymClassFormDto dto); 
        Task UpdateAsync(GymClassFormDto dto); 
        Task DeleteAsync(int id);
        Task<GymClassDetailsDto> GetDetailsAsync(int id);
        Task<EnrollmentResultDto> EnrollMemberAsync(int classId, int memberId); 
        Task<EnrollmentResultDto> UnenrollMemberAsync(int classId, int memberId); 
    }
}
