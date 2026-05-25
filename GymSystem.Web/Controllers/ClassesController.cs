using GymSystem.BLL.Services.Interfaces;
using GymSystem.Models.DTOs;
using GymSystem.Web.ViewModels.Classes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GymSystem.Web.Controllers
{
    [Authorize(Roles ="Admin")]
    public class ClassesController : Controller
    {
        private readonly IGymClassService _classService;

        public ClassesController(IGymClassService classService)
        {
            _classService = classService;
        }

        public async Task<IActionResult> Index()
        {
            var dtos = await _classService.GetAllAsync();

            var viewModels = dtos.Select(dto => new GymClassListViewModel
            {
                Id = dto.Id,
                Name = dto.Name,
                TrainerName = dto.TrainerName,
                TrainerPhotoPath = dto.TrainerPhotoPath,
                CategoryName = dto.CategoryName,
                StartTime = dto.StartTime,
                EndTime = dto.EndTime,
                Capacity = dto.Capacity,
                EnrolledCount = dto.EnrolledCount
            });

            return View(viewModels);
        }

        public async Task<IActionResult> Create()
        {
            var dto = await _classService.GetFormDataAsync();

            var viewModel = MapToFormViewModel(dto);

           return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(GymClassFormViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                var dto = await _classService.GetFormDataAsync();
                viewModel.Trainers = MapToSelectListItems(dto.Trainers);
                viewModel.Categories = MapToSelectListItems(dto.Categories);
                return View(viewModel);
            }
            var formDto = new GymClassFormDto
            {
                Name = viewModel.Name,
                TrainerId = viewModel.TrainerId,
                CategoryId = viewModel.CategoryId,
                StartTime = viewModel.StartTime,
                EndTime = viewModel.EndTime,
                Capacity = viewModel.Capacity
            };

            await _classService.CreateAsync(formDto);
            TempData["Success"] = $"Class '{formDto.Name}' created.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var dto = await _classService.GetFormDataAsync(id);
            if (dto.Id == 0) return NotFound();

            var viewModel = MapToFormViewModel(dto);
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(GymClassFormViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                var dto = await _classService.GetFormDataAsync();
                viewModel.Trainers = MapToSelectListItems(dto.Trainers);
                viewModel.Categories = MapToSelectListItems(dto.Categories);
                return View(viewModel);
            }

            var formDto = new GymClassFormDto
            {
                Id = viewModel.Id,
                Name = viewModel.Name,
                TrainerId = viewModel.TrainerId,
                CategoryId = viewModel.CategoryId,
                StartTime = viewModel.StartTime,
                EndTime = viewModel.EndTime,
                Capacity = viewModel.Capacity
            };

            await _classService.UpdateAsync(formDto);
            TempData["Success"] = "Class updated.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            await _classService.DeleteAsync(id);
            TempData["Success"] = "Class deleted.";
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Details(int id)
        {
            var dto = await _classService.GetDetailsAsync(id);

            if (dto == null)
                return NotFound();

            var viewModel = new GymClassDetailsViewModel
            {
                Id = dto.Id,
                Name = dto.Name,
                TrainerName = dto.TrainerName,
                TrainerPhotoPath = dto.TrainerPhotoPath,
                CategoryName = dto.CategoryName,
                StartTime = dto.StartTime,
                EndTime = dto.EndTime,
                Capacity = dto.Capacity,
                EnrolledCount = dto.EnrolledCount,
                EnrolledMembers = dto.EnrolledMembers.Select(em => new EnrolledMemberViewModel
                {
                    MemberId = em.MemberId,
                    MemberName = em.MemberName,
                    MemberEmail = em.MemberEmail,
                    MemberPhotoPath = em.MemberPhotoPath,
                    EnrolledAt = em.EnrolledAt
                }).ToList(),
                AvailableMembers = dto.AvailableMembers.Select(am =>
                    new SelectListItem(am.Name, am.Id.ToString())).ToList()
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Enroll(int classId, int memberId)
        {
            var result = await _classService.EnrollMemberAsync(classId, memberId);
            TempData[result.Success ? "Success" : "Error"] = result.Message;

            return RedirectToAction(nameof(Details), new { id = classId });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Unenroll(int classId, int memberId)
        {
            var result = await _classService.UnenrollMemberAsync(classId, memberId);
            TempData[result.Success ? "Success" : "Error"] = result.Message;

            return RedirectToAction(nameof(Details), new { id = classId });
        }
        private GymClassFormViewModel MapToFormViewModel(GymClassFormDto dto)
        {
            return new GymClassFormViewModel
            {
                Id = dto.Id,
                Name = dto.Name,
                TrainerId = dto.TrainerId,
                CategoryId = dto.CategoryId,
                StartTime = dto.StartTime,
                EndTime = dto.EndTime,
                Capacity = dto.Capacity,
                Trainers = MapToSelectListItems(dto.Trainers),
                Categories = MapToSelectListItems(dto.Categories)
            };
        }

        private IEnumerable<SelectListItem> MapToSelectListItems(IEnumerable<LookupItemDto> items)
        {
            return items.Select(i => new SelectListItem(i.Name, i.Id.ToString()));
        }
    }
}

