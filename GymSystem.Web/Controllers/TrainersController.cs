using GymSystem.BLL.Services.Interfaces;
using GymSystem.Models.DTOs;
using GymSystem.Web.Services;
using GymSystem.Web.ViewModels.Trainers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GymSystem.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class TrainersController : Controller
    {
        private readonly ITrainerService _trainerService;
        private readonly IPhotoService _photoService;
        private const string TrainerPhotosFolder = "images/trainers";

        public TrainersController(ITrainerService trainerService, IPhotoService photoService)
        {
            _trainerService = trainerService;
            _photoService = photoService;
        }

        public async Task<IActionResult> Index()
        {
            var trainers = await _trainerService.GetAllAsync();

            var viewModel = trainers.Select(t => new TrainerListViewModel
            {
                Id = t.Id,
                FullName = t.FullName,
                JobTitle = t.JobTitle,
                PhotoPath = t.PhotoPath,
                MemberCount = t.MemberCount,
                ClassCount = t.ClassCount,
                WeeklyHours = t.WeeklyHours,
                WeeklyHoursMax = t.WeeklyHoursMax,
                Rating = t.Rating,
                IsActive = t.IsActive,
                Specialties = t.Specialties
            });

            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var model = await _trainerService.GetAsync();
            var viewModel = new TrainerFormViewModel
            {
                FullName = model?.FullName ?? string.Empty,
                JobTitle = model?.JobTitle ?? string.Empty,
                ExistingPhotoPath = model?.PhotoPath,
                SelectedSpecialtyIds = model?.SelectedSpecialtyIds ?? new List<int>()
            };

            await PopulateSpecialtiesDropDownList(viewModel);
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TrainerFormViewModel model)
        {
            string? photoPath = null;
            if (model.Photo != null && model.Photo.Length > 0)
            {
                try
                {
                    var relativePath = await _photoService.SaveAsync(model.Photo, TrainerPhotosFolder);
                    photoPath = $"~/{relativePath.Replace("\\", "/").TrimStart('/')}";
                }
                catch (InvalidOperationException ex)
                {
                    ModelState.AddModelError(nameof(model.Photo), ex.Message);
                }
            }

            if (!ModelState.IsValid)
            {
                await PopulateSpecialtiesDropDownList(model);
                return View(model);
            }

            await _trainerService.CreateAsync(new TrainerFormDTO
            {
                FullName = model.FullName,
                JobTitle = model.JobTitle,
                PhotoPath = photoPath,
                SelectedSpecialtyIds = model.SelectedSpecialtyIds
            });

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var trainer = await _trainerService.GetAsync(id);
            if (trainer == null)
            {
                return NotFound();
            }

            var viewModel = new TrainerFormViewModel
            {
                Id = trainer.Id,
                FullName = trainer.FullName,
                JobTitle = trainer.JobTitle,
                ExistingPhotoPath = trainer.PhotoPath,
                SelectedSpecialtyIds = trainer.SelectedSpecialtyIds
            };

            await PopulateSpecialtiesDropDownList(viewModel);
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(TrainerFormViewModel model)
        {
            var photoPath = model.ExistingPhotoPath;
            if (model.Photo != null && model.Photo.Length > 0)
            {
                try
                {
                    var relativePath = await _photoService.SaveAsync(model.Photo, TrainerPhotosFolder);
                    photoPath = $"~/{relativePath.Replace("\\", "/").TrimStart('/')}";
                }
                catch (InvalidOperationException ex)
                {
                    ModelState.AddModelError(nameof(model.Photo), ex.Message);
                }
            }

            if (!ModelState.IsValid)
            {
                await PopulateSpecialtiesDropDownList(model);
                return View(model);
            }

            await _trainerService.UpdateAsync(new TrainerFormDTO
            {
                Id = model.Id,
                FullName = model.FullName,
                JobTitle = model.JobTitle,
                PhotoPath = photoPath,
                SelectedSpecialtyIds = model.SelectedSpecialtyIds
            });

            if (model.Photo != null && model.Photo.Length > 0)
            {
                _photoService.Delete(NormalizeRelativePath(model.ExistingPhotoPath));
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var trainer = await _trainerService.GetAsync(id);
            await _trainerService.DeleteAsync(id);
            _photoService.Delete(NormalizeRelativePath(trainer?.PhotoPath));
            return RedirectToAction(nameof(Index));
        }

        private async Task PopulateSpecialtiesDropDownList(TrainerFormViewModel model)
        {
            var specialties = await _trainerService.GetSpecialtiesLookupAsync();
            model.AllSpecialties = specialties.Select(s => new SelectListItem
            {
                Value = s.Id.ToString(),
                Text = s.Name
            }).ToList();
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
