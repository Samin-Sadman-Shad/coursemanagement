using System.Net;
using Moq;
using Shouldly;
using University.Application.Contracts.API;
using University.Application.Contracts.Persistance;
using University.Application.Exceptions;
using University.Application.Features.Student.Handlers.Commands;
using University.Application.Features.Student.Requests.Commands;
using University.Application.Models.DTOs.StudentDTOs;
using University.Domain.Entities.BaseEntities;
using University.Tests.Common;
using Xunit;

namespace University.Tests.StudentTests;

public class UpdateStudentEmailCommandHandlerTests
{
    [Fact]
    public async Task Handle_ValidUniqueEmail_UpdatesStudentAndReturnsNoContent()
    {
        // Arrange
        var studentRepo = new Mock<IStudentRepository>();
        var unitOfWork = UnitOfWorkMock.Create(studentRepo);
        var currentUser = new Mock<ICurrentUserService>();

        var studentId = Guid.NewGuid();
        var staffId = Guid.NewGuid();
        var student = EntityTestFactory.CreateStudent(studentId);
        student.Email = "old@example.com";

        var dto = new UpdateStudentEmailDto
        {
            StudentId = studentId,
            Email = "new@example.com"
        };

        currentUser.Setup(x => x.UserId).Returns(staffId);

        studentRepo
            .Setup(x => x.DoesEmailExistAsync(dto.Email, null))
            .ReturnsAsync(false);

        studentRepo
            .Setup(x => x.GetByIdAsync(studentId))
            .ReturnsAsync(student);

        unitOfWork
            .Setup(x => x.SaveChangesAsync())
            .Returns(Task.CompletedTask);

        var command = new UpdateStudentEmailCommand
        {
            StudentId = studentId,
            StudentEmailDto = dto
        };

        var handler = new UpdateStudentEmailCommandHandler(
            unitOfWork.Object,
            currentUser.Object);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccessful.ShouldBeTrue();
        result.Status.ShouldBe(HttpStatusCode.NoContent);
        result.RecordId.ShouldBe(studentId);

        student.Email.ShouldBe(dto.Email);
        student.LastModifiedById.ShouldBe(staffId);
        student.LastModifiedAt.ShouldNotBe(DateTimeOffset.MinValue);

        studentRepo.Verify(x => x.Update(student), Times.Once);
        unitOfWork.Verify(x => x.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task Handle_DuplicateEmail_ReturnsBadRequestAndDoesNotUpdate()
    {
        // Arrange
        var studentRepo = new Mock<IStudentRepository>();
        var unitOfWork = UnitOfWorkMock.Create(studentRepo);
        var currentUser = new Mock<ICurrentUserService>();

        var studentId = Guid.NewGuid();
        var student = EntityTestFactory.CreateStudent(studentId);
        student.Email = "old@example.com";

        var dto = new UpdateStudentEmailDto
        {
            StudentId = studentId,
            Email = "existing@example.com"
        };

        studentRepo
            .Setup(x => x.DoesEmailExistAsync(dto.Email, null))
            .ReturnsAsync(true);

        var command = new UpdateStudentEmailCommand
        {
            StudentId = studentId,
            StudentEmailDto = dto
        };

        var handler = new UpdateStudentEmailCommandHandler(
            unitOfWork.Object,
            currentUser.Object);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccessful.ShouldBeFalse();
        result.Status.ShouldBe(HttpStatusCode.BadRequest);
        result.Errors.ShouldNotBeEmpty();

        student.Email.ShouldBe("old@example.com");

        studentRepo.Verify(x => x.GetByIdAsync(It.IsAny<Guid>()), Times.Never);
        studentRepo.Verify(x => x.Update(It.IsAny<Student>()), Times.Never);
        unitOfWork.Verify(x => x.SaveChangesAsync(), Times.Never);
    }

    [Fact]
    public async Task Handle_StudentDoesNotExist_ThrowsFailToProcessCommandException()
    {
        // Arrange
        var studentRepo = new Mock<IStudentRepository>();
        var unitOfWork = UnitOfWorkMock.Create(studentRepo);
        var currentUser = new Mock<ICurrentUserService>();

        var studentId = Guid.NewGuid();
        var dto = new UpdateStudentEmailDto
        {
            StudentId = studentId,
            Email = "new@example.com"
        };

        studentRepo
            .Setup(x => x.DoesEmailExistAsync(dto.Email, null))
            .ReturnsAsync(false);

        studentRepo
            .Setup(x => x.GetByIdAsync(studentId))
            .ReturnsAsync((Student?)null);

        var command = new UpdateStudentEmailCommand
        {
            StudentId = studentId,
            StudentEmailDto = dto
        };

        var handler = new UpdateStudentEmailCommandHandler(
            unitOfWork.Object,
            currentUser.Object);

        // Act
        var act = () => handler.Handle(command, CancellationToken.None);

        // Assert
        await Should.ThrowAsync<FailToProcessCommandException>(act);

        studentRepo.Verify(x => x.Update(It.IsAny<Student>()), Times.Never);
        unitOfWork.Verify(x => x.SaveChangesAsync(), Times.Never);
    }
}
