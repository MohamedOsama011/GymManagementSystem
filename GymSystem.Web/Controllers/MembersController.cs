using GymSystem.BLL.Services.Interfaces;
using GymSystem.Models.DTOs;
using GymSystem.Web.Services;
using GymSystem.Web.ViewModels.Members;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GymSystem.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class MembersController : Controller
    {
        private readonly IMemberService _memberService;
        private readonly ITrainerService _trainerService;
        private readonly IPhotoService _photoService;
        private const string MemberPhotosFolder = "images/members";

        public MembersController(
            IMemberService memberService,
            ITrainerService trainerService,
            IPhotoService photoService)
        {
            _memberService = memberService;
            _trainerService = trainerService;
            _photoService = photoService;
        }

        [Authorize]
        public async Task<IActionResult> Index(string? search)
        {
            var memberDTO = await _memberService.GetAllAsync(search);

            var viewModel = memberDTO.Select(m => new MemberListViewModel
            {
                Id = m.Id,
                FullName = m.FullName,
                Email = m.Email,
                Phone = m.Phone ?? "N/A",
                PhotoPath = m.PhotoPath,
                TrainerName = m.TrainerName,
                ActivePlanName = m.ActivePlanName,
                SubscriptionStatus = m.SubscriptionStatus
            });
            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var viewModel = new MemberFormViewModel();
            await PopulateTrainersDropDownList(viewModel);

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(MemberFormViewModel model)
        {
            string? photoPath = null;
            if (model.Photo != null && model.Photo.Length > 0)
            {
                try
                {
                    var relativePath = await _photoService.SaveAsync(model.Photo, MemberPhotosFolder);
                    photoPath = $"~/{relativePath.Replace("\\", "/").TrimStart('/')}";
                }
                catch (InvalidOperationException ex)
                {
                    ModelState.AddModelError(nameof(model.Photo), ex.Message);
                }
            }

            if (!ModelState.IsValid)
            {
                await PopulateTrainersDropDownList(model);
                return View(model);
            }

            var createDto = new MemberCreateDTO
            {
                FullName = model.FullName,
                Email = model.Email,
                Phone = model.Phone,
                DateOfBirth = model.DateOfBirth ?? DateTime.Now.AddYears(-20),
                PhotoPath = photoPath,
                TrainerId = model.TrainerId
            };

            await _memberService.CreateAsync(createDto);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var memberDto = await _memberService.GetByIdAsync(id);
            if (memberDto == null) return NotFound();

            var viewModel = new MemberFormViewModel
            {
                Id = memberDto.Id,
                FullName = memberDto.FullName,
                Email = memberDto.Email,
                Phone = memberDto.Phone,
                DateOfBirth = memberDto.DateOfBirth,
                ExistingPhotoPath = memberDto.PhotoPath,
            };

            await PopulateTrainersDropDownList(viewModel);
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(MemberFormViewModel model)
        {
            var photoPath = model.ExistingPhotoPath;
            if (model.Photo != null && model.Photo.Length > 0)
            {
                try
                {
                    var relativePath = await _photoService.SaveAsync(model.Photo, MemberPhotosFolder);
                    photoPath = $"~/{relativePath.Replace("\\", "/").TrimStart('/')}";
                }
                catch (InvalidOperationException ex)
                {
                    ModelState.AddModelError(nameof(model.Photo), ex.Message);
                }
            }

            if (!ModelState.IsValid)
            {
                await PopulateTrainersDropDownList(model);
                return View(model);
            }

            var updateDto = new MemberUpdateDTO
            {
                Id = model.Id,
                FullName = model.FullName,
                Email = model.Email,
                Phone = model.Phone,
                DateOfBirth = model.DateOfBirth ?? DateTime.Now,
                PhotoPath = photoPath,
                TrainerId = model.TrainerId
            };

            await _memberService.UpdateAsync(updateDto);

            if (model.Photo != null && model.Photo.Length > 0)
            {
                _photoService.Delete(NormalizeRelativePath(model.ExistingPhotoPath));
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var memberDto = await _memberService.GetByIdAsync(id);
            await _memberService.DeleteAsync(id);
            _photoService.Delete(NormalizeRelativePath(memberDto?.PhotoPath));

            return RedirectToAction(nameof(Index));
        }

        private async Task PopulateTrainersDropDownList(MemberFormViewModel model)
        {
            var trainers = await _trainerService.GetAllAsync();
            model.Trainers = trainers.Select(t => new SelectListItem
            {
                Text = t.FullName,
                Value = t.Id.ToString()
            });
        }

        private static string? NormalizeRelativePath(string? storedPath)
        {
            if (string.IsNullOrWhiteSpace(storedPath))
            {
                return null;
            }

            return storedPath
                .Replace("\\", "/")
                .TrimStart('~', '/');
        }
    }
}
