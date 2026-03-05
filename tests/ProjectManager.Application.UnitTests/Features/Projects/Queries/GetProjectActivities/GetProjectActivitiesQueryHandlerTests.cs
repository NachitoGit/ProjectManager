using AutoMapper;
using FluentAssertions;
using Moq;
using ProjectManager.Application.Features.Projects.Queries.GetProjectActivities;
using ProjectManager.Domain.Entities;
using ProjectManager.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManager.Application.UnitTests.Features.Projects.Queries.GetProjectActivities
{
    public class GetProjectActivitiesQueryHandlerTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<ICurrentUserService> _currentUserServiceMock;
        private readonly GetProjectActivitiesQueryHandler _handler;

        public GetProjectActivitiesQueryHandlerTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _mapperMock = new Mock<IMapper>();
            _currentUserServiceMock = new Mock<ICurrentUserService>();

            _handler = new GetProjectActivitiesQueryHandler(
                _unitOfWorkMock.Object,
                _mapperMock.Object,
                _currentUserServiceMock.Object);
        }

        [Fact]
        public async Task Handle_UserIsNotMember_ShouldThrowUnauthorizedAccessException()
        {
            var projectId = 1;
            var userId = "user-hacker-id";

            var query = new GetProjectActivitiesQuery { ProjectId = projectId };

            // Simulamos que el servicio de usuario devuelve el ID del "hacker"
            _currentUserServiceMock.Setup(x => x.UserId).Returns(userId);

            // Simulamos que el repositorio de miembros dice que NO está en el proyecto
            _unitOfWorkMock.Setup(x => x.ProjectMembers.IsUserInProjectAsync(projectId, userId))
                .ReturnsAsync(false);

            // 2. ACT & 3. ASSERT
            // Verificamos que al ejecutar, se lance la excepción de seguridad
            await _handler.Invoking(x => x.Handle(query, default))
                .Should().ThrowAsync<UnauthorizedAccessException>()
                .WithMessage("No tienes permiso para ver el historial de este proyecto.");

            // Verificamos que nunca se llegó a llamar al repositorio de actividades (por seguridad)
            _unitOfWorkMock.Verify(x => x.ProjectActivities.GetByProjectIdAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public async  Task Handle_UserIsMember_ShouldThrowActivities()
        {
            var projectId = 1;
            var userId = "correct-user";

            var query = new GetProjectActivitiesQuery { ProjectId= projectId, PageNumber = 1, PageSize = 10 };

            var fakeActivities = new List<ProjectActivity>
            {
                new ProjectActivity { Message = "Test 1", ActivityType = "Type 1" },
                new ProjectActivity { Message = "Test 2", ActivityType = "Type 2" }
            };

            _currentUserServiceMock.Setup(x => x.UserId).Returns(userId);

            _unitOfWorkMock.Setup(x => x.ProjectMembers.IsUserInProjectAsync(projectId, userId))
                .ReturnsAsync(true);

            _unitOfWorkMock.Setup(x => x.ProjectActivities.GetByProjectIdAsync(projectId, query.PageNumber, query.PageSize))
            .ReturnsAsync(fakeActivities);

            var expectedDtos = new List<ProjectActivityDto>
            {
                new ProjectActivityDto { Message = "Test 1" },
                new ProjectActivityDto { Message = "Test 2" }
            };

            _mapperMock.Setup(m => m.Map<IEnumerable<ProjectActivityDto>>(fakeActivities))
                .Returns(expectedDtos);

            var result = await _handler.Handle(query, default);

            result.Should().NotBeNull();
            result.Should().HaveCount(2);
            result.First().Message.Should().Be("Test 1");

            _unitOfWorkMock.Verify(x => x.ProjectActivities.GetByProjectIdAsync(projectId, 1, 10), Times.Once);

        }
    }
}
