using GymSystem.BLL.Services.Interfaces;
using GymSystem.Models.DTOs;
using GymSystem.Web.ViewModels.Subscriptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GymSystem.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class SubscriptionsController : Controller
    {
        private readonly ISubscriptionService _subscriptionService;

        public SubscriptionsController(ISubscriptionService subscriptionService)
        {
            _subscriptionService = subscriptionService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var subscriptions = await _subscriptionService.GetAllAsync();
            ViewBag.PageTitle = "All Subscriptions";
            ViewBag.PageSubtitle = "Complete subscription history and current status";
            return View(subscriptions);
        }

        [HttpGet]
        public async Task<IActionResult> ExpiringSoon(int daysAhead = 7)
        {
            var subscriptions = await _subscriptionService.GetExpiringSoonAsync(daysAhead);
            ViewBag.PageTitle = $"Subscriptions expiring in the next {daysAhead} days";
            ViewBag.PageSubtitle = "Subscriptions approaching renewal";
            ViewBag.DaysAhead = daysAhead;
            return View("Index", subscriptions);
        }

        [HttpGet]
        public async Task<IActionResult> Assign(int memberId)
        {
            var dto = await _subscriptionService.GetAssignAsync(memberId);
            if (dto == null)
            {
                return NotFound();
            }

            return View(ToViewModel(dto));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Assign(SubscriptionAssignViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var dto = await _subscriptionService.GetAssignAsync(model.MemberId);
                if (dto == null)
                {
                    return NotFound();
                }

                return View(ApplySelection(ToViewModel(dto), model));
            }

            await _subscriptionService.AssignAsync(new SubscriptionAssignRequestDto
            {
                MemberId = model.MemberId,
                PlanId = model.SelectedPlanId,
                StartDate = model.StartDate
            });

            return RedirectToAction(nameof(Index));
        }

        private static SubscriptionAssignViewModel ToViewModel(SubscriptionAssignDto dto)
            => new SubscriptionAssignViewModel
            {
                MemberId = dto.MemberId,
                MemberName = dto.MemberName,
                CurrentSubscription = dto.CurrentSubscription,
                AvailablePlans = dto.AvailablePlans.Select(p => new SelectListItem
                {
                    Value = p.Id.ToString(),
                    Text = $"{p.Name} - {p.Price:C}"
                }).ToList()
            };

        private static SubscriptionAssignViewModel ApplySelection(
            SubscriptionAssignViewModel viewModel,
            SubscriptionAssignViewModel submitted)
        {
            viewModel.SelectedPlanId = submitted.SelectedPlanId;
            viewModel.StartDate = submitted.StartDate;
            return viewModel;
        }
    }
}
