using ProjectManager.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManager.Domain.Interfaces
{
    public interface IProjectActivityRepository
    {
        Task AddAsync(ProjectActivity activity);
        Task<IEnumerable<ProjectActivity>> GetByProjectIdAsync(int projectId, int pageNumber, int pageSize);
    }
}
