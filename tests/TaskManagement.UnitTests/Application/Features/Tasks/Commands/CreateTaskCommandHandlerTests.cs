using Application.Features.Tasks.Commands;
using Application.Interfaces;
using Domain.Interfaces;
using FluentAssertions;
using MassTransit;
using Moq;
using Xunit;

namespace TaskManagement.UnitTests.Application.Features.Tasks.Commands
{
    public class CreateTaskCommandHandlerTests
    {
        private readonly Mock<ITaskRepository> _mockTaskRepository;
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<ICurrentUserService> _mockCurrentUserService;
        private readonly Mock<IPublishEndpoint> _mockPublishEndpoint;
        private readonly Mock<ITaskNotifier> _mockNotifier;
        private readonly CreateTaskCommandHandler _handler;

        public CreateTaskCommandHandlerTests()
        {
            _mockTaskRepository = new Mock<ITaskRepository>();
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockCurrentUserService = new Mock<ICurrentUserService>();
            _mockPublishEndpoint = new Mock<IPublishEndpoint>();
            _mockNotifier = new Mock<ITaskNotifier>();

            _handler = new CreateTaskCommandHandler(
                _mockTaskRepository.Object,
                _mockUnitOfWork.Object,
                _mockCurrentUserService.Object,
                _mockPublishEndpoint.Object,
                _mockNotifier.Object
            );
        }

        [Fact]
        public async System.Threading.Tasks.Task Handle_ShouldCreateTask_WhenRequestIsValid()
        {
            // Arrange
            var command = new CreateTaskCommand
            {
                Title = "Test Task",
                Description = "Test Description",
                Status = Domain.Enums.TaskStatus.Todo,
                AssignedUserId = 1,
                TeamId = 10,
                DueDate = DateTime.Now.AddDays(1)
            };

            var userId = 100;
            _mockCurrentUserService.Setup(s => s.UserId).Returns(userId);
            _mockTaskRepository.Setup(r => r.AddAsync(It.IsAny<Domain.Entities.Task>(), It.IsAny<CancellationToken>()))
                .Callback<Domain.Entities.Task, CancellationToken>((t, c) => t.Id = 999) // Simulate ID generation
                .Returns((Domain.Entities.Task t, CancellationToken c) => System.Threading.Tasks.Task.FromResult(t));

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().Be(999);

            _mockTaskRepository.Verify(r => r.AddAsync(It.Is<Domain.Entities.Task>(t =>
                t.Title == command.Title &&
                t.Description == command.Description &&
                t.TeamId == command.TeamId &&
                t.CreatedByUserId == userId
            ), It.IsAny<CancellationToken>()), Times.Once);

            _mockUnitOfWork.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
            
            _mockPublishEndpoint.Verify(p => p.Publish(It.IsAny<object>(), It.IsAny<CancellationToken>()), Times.Once); // TaskCreatedMessage is strict, verifying object for now

            _mockNotifier.Verify(n => n.NotifyTaskAssignedAsync(command.AssignedUserId.Value, 999, command.Title, It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
