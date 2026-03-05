using AutoMapper;
using FluentAssertions;
using Moq;
using ProjectManager.Application.Features.TaskItems.Commands.CreateTaskItem;
using ProjectManager.Domain.Entities;
using ProjectManager.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManager.Application.UnitTests.Features.Tasks.Commands.CreateTasks
{
    public class CreateTaskItemCommandHandlerTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<ICurrentUserService> _currentUserServiceMock;
        private readonly Mock<IPermissionService> _permissionServiceMock;
        private readonly CreateTaskItemCommandHandler _handler;

        public CreateTaskItemCommandHandlerTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _mapperMock = new Mock<IMapper>();
            _currentUserServiceMock = new Mock<ICurrentUserService>();
            _permissionServiceMock = new Mock<IPermissionService>();

            _handler = new CreateTaskItemCommandHandler(
                _unitOfWorkMock.Object,
                _mapperMock.Object,
                _currentUserServiceMock.Object,
                _permissionServiceMock.Object);
        }

        [Fact]
        public async Task Handle_ValidRequest_ShouldAddTaskAndSave()
        {
            var projectId = 1;
            var command = new CreateTaskItemCommand { Title = "Nueva Tarea", ProjectId = projectId, Priority = "Low" };
            var userId = "user-123";

            var fakeProject = new Project { Id = projectId, Name = "Proyecto Existente" };
            var taskEntity = new TaskItem { Id = 100, Title = command.Title, ProjectId = command.ProjectId };

            _unitOfWorkMock.Setup(x => x.Projects.GetByIdAsync(projectId))
                .ReturnsAsync(fakeProject);

            _currentUserServiceMock.Setup(x => x.UserId).Returns(userId);

            _permissionServiceMock.Setup(x => x.CanManageTasksAsync(command.ProjectId, userId))
                .ReturnsAsync(true);

            _mapperMock.Setup(m => m.Map<TaskItem>(It.IsAny<CreateTaskItemCommand>()))
                .Returns(taskEntity);

            _unitOfWorkMock.Setup(x => x.Tasks.AddAsync(It.IsAny<TaskItem>()))
                .Returns(Task.CompletedTask);

            _unitOfWorkMock.Setup(x => x.CommitAsync())
                .ReturnsAsync(1);

            var result = await _handler.Handle(command, CancellationToken.None);

            result.Should().BeGreaterThan(0);
        }
    }
}
