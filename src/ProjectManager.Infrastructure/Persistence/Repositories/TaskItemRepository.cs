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
    public class TaskItemRepository : ITaskItemRepository
    {
        private readonly ApplicationDbContext _context;

        public TaskItemRepository (ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync (TaskItem taskItem)
        {
            await _context.TaskItems.AddAsync (taskItem);
        }

        public async Task<TaskItem> GetByIdAsync (int id)
        {
            return await _context.TaskItems.FindAsync (id);
        }

        public async Task<(IReadOnlyList<TaskItem> Items, int TotalCount)> GetPaginatedByProjectIdAsync(
            int projectId,
            int pageNumber,
            int pageSize,
            bool? isCompleted,
            string? searchTerm)
        {
            var query = _context.TaskItems
                .Include(t => t.AssignedTo)
                .Where(t => t.ProjectId == projectId).AsQueryable();

            if (isCompleted.HasValue)
            {
                query = query.Where(t => t.IsCompleted == isCompleted.Value);
            }

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                query = query.Where(t => t.Title.Contains(searchTerm));
            }

            var totalCount = await query.CountAsync();

            var items = await query
                .OrderByDescending( t=> t.CreatedAt)
                .Skip((pageNumber - 1) * pageSize) 
                .Take(pageSize)                    
                .ToListAsync();

            return (items, totalCount);
        }

        public void Update(TaskItem taskItem)
        {
            _context.TaskItems.Update(taskItem);
        }

        public void Delete(TaskItem taskItem)
        {
            _context.TaskItems.Remove(taskItem);
        }


    }
}
