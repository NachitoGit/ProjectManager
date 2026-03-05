using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManager.Domain.Interfaces
{
    public interface IPermissionService
    {
        Task<bool> IsOwnerAsync(int projectId, string? userId);


        Task<bool> CanManageTasksAsync(int projectId, string? userId);


        Task<bool> IsMemberAsync(int projectId, string? userId);
    }
}

