using ProjectManager.Domain.Entities;
using System;

namespace ProjectManager.Domain.Interfaces
{
    public interface IProjectRepository
    {
        Task<Project> GetByIdAsync(int id);
        Task<List<Project>> GetAllAsync();
        Task AddAsync(Project project);
        Task UpdateAsync(Project project);
        Task DeleteAsync(Project project);
        Task<bool> IsNameUniqueAsync(string name, string userId);
        Task<IEnumerable<Project>> GetProjectsByUserIdAsync(string id);
    }
}
