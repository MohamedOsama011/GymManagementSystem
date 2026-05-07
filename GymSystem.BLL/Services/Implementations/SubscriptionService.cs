using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GymSystem.BLL.Services.Interfaces;
using GymSystem.DAL.UnitOfWork.Interfaces;
using GymSystem.Models.DTOs;
using GymSystem.Models.Entities;

namespace GymSystem.BLL.Services.Implementations
{
    public class SubscriptionService : ISubscriptionService
    {
        private readonly IUnitOfWork _uow;

        public SubscriptionService(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<IEnumerable<SubscriptionDto>> GetAllAsync()
        {
            var subscriptions = await _uow.Subscriptions.GetAllWithDetailsAsync();
            return subscriptions.Select(MapToDto);
        }

        public async Task<SubscriptionAssignDto?> GetAssignAsync(int memberId)
        {
            var member = await _uow.Members.GetWithDetailsAsync(memberId);
            if (member == null)
            {
                return null;
            }

            var activeSubscription = member.Subscriptions
                .Where(s => s.Status == "Active")
                .OrderByDescending(s => s.StartDate)
                .FirstOrDefault();

            var plans = await _uow.MembershipPlans.GetAllAsync();

            return new SubscriptionAssignDto
            {
                MemberId = member.Id,
                MemberName = member.FullName,
                CurrentSubscription = activeSubscription == null
                    ? null
                    : MapToDto(activeSubscription),
                AvailablePlans = plans
                    .OrderBy(p => p.Name)
                    .Select(MapPlanToDto)
                    .ToList()
            };
        }

        public async Task AssignAsync(SubscriptionAssignRequestDto dto)
        {
            var member = await _uow.Members.GetByIdAsync(dto.MemberId);
            if (member == null)
            {
                throw new ArgumentException("Member not found.", nameof(dto.MemberId));
            }

            var plan = await _uow.MembershipPlans.GetByIdAsync(dto.PlanId);
            if (plan == null)
            {
                throw new ArgumentException("Plan not found.", nameof(dto.PlanId));
            }

            var activeSubscription = await _uow.Subscriptions.GetActiveSubscriptionAsync(dto.MemberId);
            if (activeSubscription != null)
            {
                activeSubscription.Status = "Expired";
                _uow.Subscriptions.Update(activeSubscription);
            }

            var startDate = dto.StartDate ?? DateTime.Today;
            var subscription = new Subscription
            {
                MemberId = dto.MemberId,
                PlanId = dto.PlanId,
                StartDate = startDate,
                EndDate = startDate.AddDays(plan.DurationInDays),
                Status = "Active"
            };

            await _uow.Subscriptions.AddAsync(subscription);
            await _uow.SaveChangesAsync();
        }

        public async Task<IEnumerable<SubscriptionDto>> GetExpiringSoonAsync(int daysAhead = 7)
        {
            var subscriptions = await _uow.Subscriptions.GetExpiringSoonAsync(daysAhead);
            return subscriptions.Select(MapToDto);
        }

        private static SubscriptionDto MapToDto(Subscription subscription)
            => new SubscriptionDto
            {
                Id = subscription.Id,
                MemberId = subscription.MemberId,
                MemberName = subscription.Member?.FullName ?? string.Empty,
                PlanId = subscription.PlanId,
                PlanName = subscription.Plan?.Name ?? string.Empty,
                PlanPrice = subscription.Plan?.Price ?? 0m,
                StartDate = subscription.StartDate,
                EndDate = subscription.EndDate,
                Status = subscription.Status
            };

        private static MembershipPlanDto MapPlanToDto(MembershipPlan plan)
            => new MembershipPlanDto
            {
                Id = plan.Id,
                Name = plan.Name,
                Price = plan.Price,
                DurationInDays = plan.DurationInDays
            };
    }
}
