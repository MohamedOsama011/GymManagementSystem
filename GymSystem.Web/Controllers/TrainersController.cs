using GymSystem.BLL.Services.Interfaces;
using GymSystem.Models.DTOs;
using GymSystem.Web.ViewModels.Trainers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
                JobTitle = t.JobTitle
            });

            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var model = await _trainerService.GetAsync();
            return View(new TrainerFormViewModel
            {
                FullName = model?.FullName ?? string.Empty,
                JobTitle = model?.JobTitle ?? string.Empty
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TrainerFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            await _trainerService.CreateAsync(new TrainerFormDTO
            {
                FullName = model.FullName,
                JobTitle = model.JobTitle
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

            return View(new TrainerFormViewModel
            {
                Id = trainer.Id,
                FullName = trainer.FullName,
                JobTitle = trainer.JobTitle
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(TrainerFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            await _trainerService.UpdateAsync(new TrainerFormDTO
            {
                Id = model.Id,
                FullName = model.FullName,
                JobTitle = model.JobTitle
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
    }
}
