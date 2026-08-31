using System.Net;
using Moq;
using Shouldly;
using University.Application.Contracts.API;
using University.Application.Contracts.Identity;
using University.Application.Contracts.Persistance;
using University.Application.Models.DTOs.StudentDTOs;
using University.Tests.Common;
using University.Domain.Entities.BaseEntities;
using University.Domain.Entities.JunctionEntities;
using Xunit;
using University.Application.Models.DTOs.Staff;
using University.Application.Features.Student.Handlers.Commands;
using University.Application.Features.Student.Requests.Commands;

namespace University.Tests.StudentTests;

public class CreateStudentCommandHandlerTests
{
    [Fact]
    public async Task Handle_ValidStudent_CreatesStudentAndReturnsCreated()
    {
        // Arrange
        var studentRepo = new Mock<IStudentRepository>();
        var unitOfWork = UnitOfWorkMock.Create(studentRepo);
        var userService = new Mock<IUserService>();
        var currentUser = new Mock<ICurrentUserService>();

        var staffId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var dto = new CreateStudentDto
        {
            Name = "Samin Student",
            Roll = 101,
            Email = "samin.student@example.com"
        };

        var createdStudent = EntityTestFactory.CreateStudent(userId);
        createdStudent.Name = dto.Name;
        createdStudent.Roll = dto.Roll;
        createdStudent.Email = dto.Email;
        createdStudent.CreatedById = staffId;
        createdStudent.LastModifiedById = staffId;

        currentUser.Setup(x => x.UserId).Returns(staffId);
        userService
            .Setup(x => x.GetStaffByIdAsync(staffId))
            .ReturnsAsync(new StaffDto());

        userService
            .Setup(x => x.CreateStudentAccountAsync(dto.Email, dto.Name))
            .ReturnsAsync((userId, "reset-token"));

        studentRepo
            .Setup(x => x.DoesEmailExistAsync(dto.Email, null))
            .ReturnsAsync(false);

        studentRepo
            .Setup(x => x.CreateAsync(It.IsAny<Student>()))
            .ReturnsAsync(createdStudent);


        unitOfWork
            .Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var command = new CreateStudentCommand
        {
            CreateStudentDto = dto
        };

        var handler = new CreateStudentCommandHandler(
            unitOfWork.Object,
            userService.Object,
            currentUser.Object);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccessful.ShouldBeTrue();
        result.Status.ShouldBe(HttpStatusCode.Created);
        result.RecordId.ShouldBe(userId);
        result.Record.ShouldNotBeNull();
        result.Record.Name.ShouldBe(dto.Name);
        result.Record.Roll.ShouldBe(dto.Roll);
        result.Record.Email.ShouldBe(dto.Email);
        result.PasswordResetToken.ShouldBe("reset-token");

        userService.Verify(
            x => x.CreateStudentAccountAsync(dto.Email, dto.Name),
            Times.Once);

        studentRepo.Verify(
            x => x.CreateAsync(It.Is<Student>(s =>
                s.UserId == userId &&
                s.Name == dto.Name &&
                s.Roll == dto.Roll &&
                s.Email == dto.Email &&
                s.CreatedById == staffId &&
                s.LastModifiedById == staffId)),
            Times.Once);

        unitOfWork.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_DuplicateEmail_ReturnsBadRequestAndDoesNotCreateAccount()
    {
        // Arrange
        var studentRepo = new Mock<IStudentRepository>();
        var unitOfWork = UnitOfWorkMock.Create(studentRepo);
        var userService = new Mock<IUserService>();
        var currentUser = new Mock<ICurrentUserService>();

        var dto = new CreateStudentDto
        {
            Name = "Samin Student",
            Roll = 101,
            Email = "existing@example.com"
        };

        studentRepo
            .Setup(x => x.DoesEmailExistAsync(dto.Email, null))
            .ReturnsAsync(true);

        var command = new CreateStudentCommand
        {
            CreateStudentDto = dto
        };

        var handler = new CreateStudentCommandHandler(
            unitOfWork.Object,
            userService.Object,
            currentUser.Object);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccessful.ShouldBeFalse();
        result.Status.ShouldBe(HttpStatusCode.BadRequest);
        result.Errors.ShouldNotBeEmpty();

        userService.Verify(
            x => x.CreateStudentAccountAsync(It.IsAny<string>(), It.IsAny<string>()),
            Times.Never);

        studentRepo.Verify(
            x => x.CreateAsync(It.IsAny<Student>()),
            Times.Never);

        unitOfWork.Verify(x => x.BeginTransactionAsync(It.IsAny<CancellationToken>()), Times.Never);
    }
}
