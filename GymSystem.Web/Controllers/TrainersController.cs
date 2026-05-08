using GymSystem.BLL.Services.Interfaces;
using GymSystem.Models.DTOs;
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

        public TrainersController(ITrainerService trainerService)
        {
            _trainerService = trainerService;
        }

        public async Task<IActionResult> Index()
        {
            var trainers = await _trainerService.GetAllAsync();

            var viewModel = trainers.Select(t => new TrainerListViewModel
            {
                Id = t.Id,
                FullName = t.FullName,
                JobTitle = t.JobTitle,
                MemberCount = t.MemberCount,
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
                SelectedSpecialtyIds = model?.SelectedSpecialtyIds ?? new List<int>()
            };

            await PopulateSpecialtiesDropDownList(viewModel);
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TrainerFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                await PopulateSpecialtiesDropDownList(model);
                return View(model);
            }

            await _trainerService.CreateAsync(new TrainerFormDTO
            {
                FullName = model.FullName,
                JobTitle = model.JobTitle,
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
                SelectedSpecialtyIds = trainer.SelectedSpecialtyIds
            };

            await PopulateSpecialtiesDropDownList(viewModel);
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(TrainerFormViewModel model)
        {
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
                SelectedSpecialtyIds = model.SelectedSpecialtyIds
            });

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            await _trainerService.DeleteAsync(id);
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
    }
}
