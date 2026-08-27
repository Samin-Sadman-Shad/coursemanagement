using System.Net;
using Moq;
using Shouldly;
using University.Application.Contracts.API;
using University.Application.Contracts.Identity;
using University.Application.Contracts.Persistance;
using University.Application.Exceptions;
using University.Application.Features.Student.Handlers.Queries;
using University.Application.Features.Student.Requests.Queries;
using University.Application.Models.DTOs.Staff;
using University.Domain.Entities.BaseEntities;
using University.Tests.Common;
using Xunit;

namespace University.Tests.StudentTests;

public class GetPeersByStudentIdRequestHandlerTests
{
    [Fact]
    public async Task Handle_StudentHasPeers_ReturnsMappedPeers()
    {
        // Arrange
        var studentRepo = new Mock<IStudentRepository>();
        var unitOfWork = UnitOfWorkMock.Create(studentRepo);
        var userService = new Mock<IUserService>();
        var currentUser = new Mock<ICurrentUserService>();

        var currentStudentId = Guid.NewGuid();

        var peer1 = EntityTestFactory.CreateStudent();
        peer1.Name = "Peer One";
        peer1.Roll = 10;
        peer1.CreatedById = Guid.NewGuid();

        var peer2 = EntityTestFactory.CreateStudent();
        peer2.Name = "Peer Two";
        peer2.Roll = 11;
        peer2.CreatedById = Guid.NewGuid();

        currentUser.Setup(x => x.UserId).Returns(currentStudentId);

        studentRepo
            .Setup(x => x.GetPeersByStudentIdAsync(currentStudentId))
            .ReturnsAsync(new List<Student> { peer1, peer2 });

        userService
            .Setup(x => x.GetStaffByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(new StaffDto());

        var request = new GetPeersByStudentIdRequest();

        var handler = new GetPeersByStudentIdRequestHandler(
            unitOfWork.Object,
            userService.Object,
            currentUser.Object);

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        result.IsSuccessful.ShouldBeTrue();
        result.Status.ShouldBe(HttpStatusCode.OK);
        result.Records.Count.ShouldBe(2);
        result.Records.Select(x => x.Name).ShouldBe(new[] { "Peer One", "Peer Two" });

        studentRepo.Verify(
            x => x.GetPeersByStudentIdAsync(currentStudentId),
            Times.Once);
    }

    [Fact]
    public async Task Handle_NoPeers_ReturnsEmptyList()
    {
        // Arrange
        var studentRepo = new Mock<IStudentRepository>();
        var unitOfWork = UnitOfWorkMock.Create(studentRepo);
        var userService = new Mock<IUserService>();
        var currentUser = new Mock<ICurrentUserService>();

        var currentStudentId = Guid.NewGuid();
        currentUser.Setup(x => x.UserId).Returns(currentStudentId);

        studentRepo
            .Setup(x => x.GetPeersByStudentIdAsync(currentStudentId))
            .ReturnsAsync(new List<Student>());

        var request = new GetPeersByStudentIdRequest();

        var handler = new GetPeersByStudentIdRequestHandler(
            unitOfWork.Object,
            userService.Object,
            currentUser.Object);

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        result.IsSuccessful.ShouldBeTrue();
        result.Status.ShouldBe(HttpStatusCode.OK);
        result.Records.ShouldBeEmpty();

        userService.Verify(
            x => x.GetStaffByIdAsync(It.IsAny<Guid>()),
            Times.Never);
    }

    [Fact]
    public async Task Handle_RepositoryThrows_ThrowsFailToProcessQueryException()
    {
        // Arrange
        var studentRepo = new Mock<IStudentRepository>();
        var unitOfWork = UnitOfWorkMock.Create(studentRepo);
        var userService = new Mock<IUserService>();
        var currentUser = new Mock<ICurrentUserService>();

        var currentStudentId = Guid.NewGuid();
        currentUser.Setup(x => x.UserId).Returns(currentStudentId);

        studentRepo
            .Setup(x => x.GetPeersByStudentIdAsync(currentStudentId))
            .ThrowsAsync(new InvalidOperationException("Database failure"));

        var request = new GetPeersByStudentIdRequest();

        var handler = new GetPeersByStudentIdRequestHandler(
            unitOfWork.Object,
            userService.Object,
            currentUser.Object);

        // Act
        var act = () => handler.Handle(request, CancellationToken.None);

        // Assert
        await Should.ThrowAsync<FailToProcessQueryException>(act);
    }
}
