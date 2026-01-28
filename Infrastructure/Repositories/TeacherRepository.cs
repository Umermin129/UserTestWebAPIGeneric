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
    public class TeacherRepository : IRepository<Teacher>
    {
        private readonly ApplicationDbContext _context;

        public TeacherRepository(ApplicationDbContext dbContext)
        {
            _context = dbContext;
        }
        public async Task<Teacher> Create(Teacher user)
        {
            await _context.Teachers.AddAsync(user);
            return user;
        }
        public async Task<Teacher?> GetAsync(Guid id)
        {
            return await _context.Teachers
                .                 Include(t => t.User)
                                 .AsNoTracking()
                                 .FirstOrDefaultAsync(u => u.Id == id);
        }
        public async Task<List<Teacher>> GetAllAsync()
        {
            return await _context.Teachers.Include(t => t.User).ToListAsync();
        }
    }
}
