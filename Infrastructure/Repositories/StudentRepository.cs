using Domain.Entities;
using Infrastructure.Data;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class StudentRepository : IRepository<Student>
    {
        private readonly ApplicationDbContext _context;
        public StudentRepository(ApplicationDbContext dbContext)
        {
            _context = dbContext;
        }
        public async Task<Student> Create(Student user)
        {
            await _context.Students.AddAsync(user);
            return user;
        }
        public async Task<Student?> GetAsync(Guid id)
        {
            return await _context.Students.
                Include(s => s.User).
                AsNoTracking().
                FirstOrDefaultAsync(s => s.Id == id);
        }
        public async Task<List<Student>> GetAllAsync()
        {
            return await _context.Students.ToListAsync();
        }
    }
}
