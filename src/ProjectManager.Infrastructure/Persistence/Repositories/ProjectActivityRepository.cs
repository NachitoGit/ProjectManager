using ProjectManager.Domain.Entities;
using ProjectManager.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;


namespace ProjectManager.Infrastructure.Persistence.Repositories
{
    public class ProjectActivityRepository :  IProjectActivityRepository
    {
        private readonly ApplicationDbContext _context;

    public ProjectActivityRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(ProjectActivity activity)
    {
        await _context.ProjectActivities.AddAsync(activity);
    }

    public async Task<IEnumerable<ProjectActivity>> GetByProjectIdAsync(int projectId, int pageNumber, int pageSize)
    {
        return await _context.ProjectActivities
            .Where(a => a.ProjectId == projectId)
            .OrderByDescending(a => a.OccurredAt)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }
    }
}
