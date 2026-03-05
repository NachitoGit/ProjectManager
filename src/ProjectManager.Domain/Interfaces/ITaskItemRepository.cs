using ProjectManager.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManager.Domain.Interfaces
{
    public interface ITaskItemRepository
    {
        Task AddAsync(TaskItem taskItem);

        Task<TaskItem> GetByIdAsync(int id);

        Task<(IReadOnlyList<TaskItem> Items, int TotalCount)> GetPaginatedByProjectIdAsync(
        int projectId,
        int pageNumber,
        int pageSize,
        bool? isCompleted,
        string? searchTerm);

        void Update(TaskItem taskItem);
        void Delete(TaskItem taskItem);
    }
}
