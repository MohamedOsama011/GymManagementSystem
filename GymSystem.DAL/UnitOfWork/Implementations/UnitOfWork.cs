using GymSystem.DAL.Data;
using GymSystem.DAL.Repositories.Implementations;
using GymSystem.DAL.Repositories.Interfaces;
using GymSystem.DAL.UnitOfWork.Interfaces;
using GymSystem.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace GymSystem.DAL.UnitOfWork.Implementations
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        public IMemberRepository Members { get; private set; }
        public ITrainerRepository Trainers { get; private set; }

        public UnitOfWork(AppDbContext context)
        {
            _context = context;

            Members = new MemberRepository(_context);
            Trainers = new TrainerRepository(_context);
        }
        public async Task<int> SaveChangesAsync() => await _context.SaveChangesAsync();

        public void Dispose() => _context.Dispose();
    }
}
