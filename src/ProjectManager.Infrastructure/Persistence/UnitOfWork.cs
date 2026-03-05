using ProjectManager.Domain.Interfaces;
using ProjectManager.Infrastructure.Persistence.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManager.Infrastructure.Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        private IProjectRepository _projectRepository;
        private ITaskItemRepository _taskRepository;
        private IProjectMemberRepository _memberRepository;
        private IProjectActivityRepository _activityRepository;

        public UnitOfWork(ApplicationDbContext context) => _context = context;

        public IProjectRepository Projects => _projectRepository ??= new ProjectRepository(_context);
        public ITaskItemRepository Tasks => _taskRepository ??= new TaskItemRepository(_context);
        public IProjectMemberRepository ProjectMembers => _memberRepository ??= new ProjectMemberRepository(_context);
        public IProjectActivityRepository ProjectActivities => _activityRepository ??= new ProjectActivityRepository(_context);

        public async Task<int> CommitAsync() => await _context.SaveChangesAsync();

        public void Dispose() => _context.Dispose();
    }
}
