using GymSystem.BLL.Services.Implementations;
using GymSystem.BLL.Services.Interfaces;
using GymSystem.Models.DTOs;
using GymSystem.Web.ViewModels.Dashboard;
using GymSystem.Web.ViewModels.Members;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GymSystem.Web.Controllers
{
    public class MembersController : Controller
    {
        private readonly IMemberService _memberService;
        private readonly ITrainerService _trainerService;

        public MembersController(IMemberService memberService, ITrainerService trainerService)
        {
            _memberService = memberService;
            _trainerService = trainerService;
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
                TrainerName = m.TrainerName,
                
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
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var createDto = new MemberCreateDTO
            {
                FullName = model.FullName,
                Email = model.Email,
                Phone = model.Phone,
                DateOfBirth = model.DateOfBirth ?? DateTime.Now.AddYears(-20),
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
            };

            await PopulateTrainersDropDownList(viewModel);
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(MemberFormViewModel model)
        {
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
                TrainerId = model.TrainerId
            };

            await _memberService.UpdateAsync(updateDto);
            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await _memberService.DeleteAsync(id);
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
    }
}
