using System.Net;
using Moq;
using Shouldly;
using University.Application.Contracts.Persistance;
using University.Application.Exceptions;
using University.Application.Features.Student.Handlers.Commands;
using University.Application.Features.Student.Requests.Commands;
using University.Domain.Entities.BaseEntities;
using University.Tests.Common;
using Xunit;

namespace University.Tests.StudentTests;

public class DeleteStudentCommandHandlerTests
{
    [Fact]
    public async Task Handle_ExistingStudent_DeletesStudentAndReturnsNoContent()
    {
        // Arrange
        var studentRepo = new Mock<IStudentRepository>();
        var unitOfWork = UnitOfWorkMock.Create(studentRepo);

        var studentId = Guid.NewGuid();
        var student = EntityTestFactory.CreateStudent(studentId);

        studentRepo
            .Setup(x => x.GetByIdAsync(studentId))
            .ReturnsAsync(student);

        studentRepo
    .Setup(x => x.DeleteAsync(studentId))
    .ReturnsAsync(student);

        unitOfWork
            .Setup(x => x.SaveChangesAsync())
            .Returns(Task.CompletedTask);

        var command = new DeleteStudentCommand
        {
            StudentId = studentId
        };

        var handler = new DeleteStudentCommandHandler(unitOfWork.Object);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccessful.ShouldBeTrue();
        result.Status.ShouldBe(HttpStatusCode.NoContent);
        result.RecordId.ShouldBe(studentId);

        studentRepo.Verify(x => x.GetByIdAsync(studentId), Times.Once);
        studentRepo.Verify(x => x.DeleteAsync(studentId), Times.Once);
        unitOfWork.Verify(x => x.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task Handle_StudentDoesNotExist_ThrowsFailToProcessCommandException()
    {
        // Arrange
        var studentRepo = new Mock<IStudentRepository>();
        var unitOfWork = UnitOfWorkMock.Create(studentRepo);

        var studentId = Guid.NewGuid();

        studentRepo
            .Setup(x => x.GetByIdAsync(studentId))
            .ReturnsAsync((Student?)null);

        var command = new DeleteStudentCommand
        {
            StudentId = studentId
        };

        var handler = new DeleteStudentCommandHandler(unitOfWork.Object);

        // Act
        var act = () => handler.Handle(command, CancellationToken.None);

        // Assert
        await Should.ThrowAsync<FailToProcessCommandException>(act);

        studentRepo.Verify(x => x.DeleteAsync(It.IsAny<Guid>()), Times.Never);
        unitOfWork.Verify(x => x.SaveChangesAsync(), Times.Never);
    }
}
