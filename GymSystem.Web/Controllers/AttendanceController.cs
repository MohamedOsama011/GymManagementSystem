using GymSystem.BLL.Services.Interfaces;
using GymSystem.Models.DTOs;
using GymSystem.Web.ViewModels.Attendance;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GymSystem.Web.Controllers
{
    public class AttendanceController : Controller
    {
        private readonly IAttendanceService _attendanceService;

        public AttendanceController(IAttendanceService attendanceService)
        {
            _attendanceService = attendanceService;
        }

        // GET: Attendance
        public async Task<IActionResult> Index()
        {
            var dtos = await _attendanceService.GetTodayAsync();
            var viewModels = dtos.Select(MapToViewModel);

            ViewBag.Stats = await _attendanceService.GetStatsAsync();

            return View(viewModels);
        }

        // GET: Attendance/CheckIn
        public async Task<IActionResult> CheckIn()
        {
            var dto = await _attendanceService.GetCheckInDataAsync();

            var viewModel = new CheckInViewModel
            {
                Members = dto.Members.Select(m =>
                    new SelectListItem(m.Name, m.Id.ToString()))
            };

            return View(viewModel);
        }

        // POST: Attendance/CheckIn
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CheckIn(CheckInViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var dto = await _attendanceService.GetCheckInDataAsync();
                model.Members = dto.Members.Select(m =>
                    new SelectListItem(m.Name, m.Id.ToString()));
                return View(model);
            }

            var result = await _attendanceService.CheckInAsync(model.MemberId);

            if (result.Success)
                TempData["Success"] = result.Message;
            else
                TempData["Error"] = result.Message;

            return RedirectToAction(nameof(Index));
        }

        // POST: Attendance/CheckOut/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CheckOut(int id)
        {
            var result = await _attendanceService.CheckOutAsync(id);

            if (result.Success)
                TempData["Success"] = result.Message;
            else
                TempData["Error"] = result.Message;

            return RedirectToAction(nameof(Index));
        }

        // GET: Attendance/MemberHistory/5
        public async Task<IActionResult> MemberHistory(int memberId)
        {
            var dtos = await _attendanceService.GetByMemberAsync(memberId);
            var viewModels = dtos.Select(MapToViewModel);

            ViewData["MemberId"] = memberId;
            ViewData["MemberName"] = dtos.FirstOrDefault()?.MemberName ?? "Unknown";

            return View(viewModels);
        }

        public async Task<IActionResult> Filter(DateTime? fromDate, DateTime? toDate, int? memberId)
        {
            var filter = new AttendanceFilterDto
            {
                FromDate = fromDate,
                ToDate = toDate,
                MemberId = memberId
            };

            var dtos = await _attendanceService.GetFilteredAsync(filter);
            var viewModels = dtos.Select(MapToViewModel);

            ViewBag.FromDate = fromDate?.ToString("yyyy-MM-dd");
            ViewBag.ToDate = toDate?.ToString("yyyy-MM-dd");
            ViewBag.MemberId = memberId;

            return View("Index", viewModels);
        }

        private AttendanceViewModel MapToViewModel(AttendanceRecordDto dto) => new()
        {
            Id = dto.Id,
            MemberId = dto.MemberId,
            MemberName = dto.MemberName,
            CheckInTime = dto.CheckInTime,
            CheckOutTime = dto.CheckOutTime
        };
    }
}
