using ProjectManager.Domain.Constants;
using ProjectManager.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManager.Infrastructure.Services
{
    public class PermissionService : IPermissionService
    {
        private readonly IUnitOfWork _unitOfWork;

        public PermissionService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> IsOwnerAsync(int projectId, string? userId)
        {
            if (string.IsNullOrEmpty(userId)) return false;

            var membership = await _unitOfWork.ProjectMembers.GetByIdsAsync(projectId, userId);
            return membership?.Role == ProjectRoles.Owner;
        }

        public async Task<bool> CanManageTasksAsync(int projectId, string? userId)
        {
            if (string.IsNullOrEmpty(userId)) return false;

            var membership = await _unitOfWork.ProjectMembers.GetByIdsAsync(projectId, userId);

            // El Owner y el Contributor pueden gestionar tareas
            return membership != null &&
                   (membership.Role == ProjectRoles.Owner || membership.Role == ProjectRoles.Contributor);
        }

        public async Task<bool> IsMemberAsync(int projectId, string? userId)
        {
            if (string.IsNullOrEmpty(userId)) return false;

            var membership = await _unitOfWork.ProjectMembers.GetByIdsAsync(projectId, userId);
            return membership != null;
        }
    }
}
