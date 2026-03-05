using Microsoft.EntityFrameworkCore;
using ProjectManager.Domain.Entities;
using ProjectManager.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManager.Infrastructure.Persistence.Repositories
{
    public class ProjectMemberRepository : IProjectMemberRepository
    {
        private readonly ApplicationDbContext _context;

        public ProjectMemberRepository(ApplicationDbContext context) => _context = context;

        public async Task AddAsync(ProjectMember member) => await _context.ProjectMembers.AddAsync(member);

        public async Task<ProjectMember?> GetByIdsAsync(int projectId, string userId)
        {
            return await _context.ProjectMembers
                .FirstOrDefaultAsync(pm => pm.ProjectId == projectId && pm.UserId == userId);
        }

        public async Task<IReadOnlyList<ProjectMember>> GetByProjectIdAsync(int projectId)
        {
            return await _context.ProjectMembers
                .Include(pm => pm.User)
                .Where(pm => pm.ProjectId == projectId)
                .ToListAsync();
        }

        public void Remove(ProjectMember member) => _context.ProjectMembers.Remove(member);

        public async Task<bool> IsUserInProjectAsync(int projectId, string userId)
        {
            return await _context.ProjectMembers
                .AnyAsync(pm => pm.ProjectId == projectId && pm.UserId == userId);
        }
    }
}
