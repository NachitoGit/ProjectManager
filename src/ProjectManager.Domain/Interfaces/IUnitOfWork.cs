using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManager.Domain.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IProjectRepository Projects { get; }

        ITaskItemRepository Tasks { get; }

        IProjectMemberRepository ProjectMembers { get; }

        IProjectActivityRepository ProjectActivities { get; }

        Task<int> CommitAsync();
    }
}
