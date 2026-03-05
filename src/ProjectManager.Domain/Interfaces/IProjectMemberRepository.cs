using ProjectManager.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManager.Domain.Interfaces
{
    public interface IProjectMemberRepository
    {
        Task AddAsync(ProjectMember member);
        Task<ProjectMember?> GetByIdsAsync(int projectId, string userId);
        Task<IReadOnlyList<ProjectMember>> GetByProjectIdAsync(int projectId);
        Task<bool> IsUserInProjectAsync(int projectId, string userId);
        void Remove(ProjectMember member);

    }
}
