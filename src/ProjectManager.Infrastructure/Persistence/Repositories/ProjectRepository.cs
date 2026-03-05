using Microsoft.EntityFrameworkCore;
using ProjectManager.Domain.Constants;
using ProjectManager.Domain.Entities;
using ProjectManager.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManager.Infrastructure.Persistence.Repositories
{
    public class ProjectRepository : IProjectRepository
    {
        private readonly ApplicationDbContext _context;

        public ProjectRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task AddAsync(Project project)
        {
            await _context.Projects.AddAsync(project);
        }

        public async Task<Project> GetByIdAsync(int id) =>
            await _context.Projects
                          .Include(p => p.Tasks)
                          .Include(p => p.Members)
                          .FirstOrDefaultAsync(p => p.Id == id);

        public async Task DeleteAsync(Project project)
        {
            _context.Projects.Remove(project);
        }

        public async Task UpdateAsync(Project project)
        {
            _context.Projects.Update(project);
        }

        public async Task<List<Project>> GetAllAsync()
        {
            return await _context.Projects.ToListAsync();
        }

        public async Task<bool> IsNameUniqueAsync(string name, string userId)
        {
            return !await _context.Projects
                .AnyAsync(p => p.Name.ToLower() == name.ToLower()
                          && p.Members.Any(m => m.UserId == userId && m.Role == ProjectRoles.Owner));
        }

        public async Task<IEnumerable<Project>> GetProjectsByUserIdAsync(string userId)
        {
            return await _context.Projects
                .Where(p => p.Members.Any(m => m.UserId == userId))
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();
        }
    }
}
