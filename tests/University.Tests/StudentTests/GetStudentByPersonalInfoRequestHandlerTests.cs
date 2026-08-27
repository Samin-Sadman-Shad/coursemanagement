using System.Net;
using Moq;
using Shouldly;
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

public class GetStudentByPersonalInfoRequestHandlerTests
{
    [Fact]
    public async Task Handle_ByEmailWhenStudentExists_ReturnsStudentWithDetails()
    {
        // Arrange
        var studentRepo = new Mock<IStudentRepository>();
        var unitOfWork = UnitOfWorkMock.Create(studentRepo);
        var userService = new Mock<IUserService>();

        var studentId = Guid.NewGuid();
        var student = EntityTestFactory.CreateStudent(studentId);
        student.Email = "student@example.com";
        student.CreatedById = Guid.NewGuid();

        var course = EntityTestFactory.CreateCourse();
        var creditWork = EntityTestFactory.CreateCreditWork();

        student.CoursesEnrolled.Add(
            EntityTestFactory.CreateCourseEnrollment(
                student: student,
                course: course));

        student.CreditWorksEnrolled.Add(
            EntityTestFactory.CreateCreditWorkEnrollment(
                student: student,
                creditWork: creditWork));

        studentRepo
            .Setup(x => x.GetStudentByEmailAsync(student.Email))
            .ReturnsAsync(student);

        userService
            .Setup(x => x.GetStaffByIdAsync(student.CreatedById))
            .ReturnsAsync(new StaffDto());

        var request = new GetStudentByPersonalInfoRequest
        {
            Email = student.Email
        };

        var handler = new GetStudentByPersonalInfoRequestHandler(
            unitOfWork.Object,
            userService.Object);

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        result.IsSuccessful.ShouldBeTrue();
        result.Status.ShouldBe(HttpStatusCode.OK);
        result.Record.ShouldNotBeNull();
        result.Record.Id.ShouldBe(student.UserId);
        result.Record.Name.ShouldBe(student.Name);
        result.Record.Email.ShouldBe(student.Email);
        result.Record.Courses.Count.ShouldBe(1);
        result.Record.CreditWorks.Count.ShouldBe(1);

        studentRepo.Verify(
            x => x.GetStudentByEmailAsync(student.Email),
            Times.Once);

        studentRepo.Verify(
            x => x.GetStudentByRollAsync(It.IsAny<int?>()),
            Times.Never);
    }

    [Fact]
    public async Task Handle_ByRollWhenStudentDoesNotExist_ReturnsNotFound()
    {
        // Arrange
        var studentRepo = new Mock<IStudentRepository>();
        var unitOfWork = UnitOfWorkMock.Create(studentRepo);
        var userService = new Mock<IUserService>();

        const int roll = 999;

        studentRepo
            .Setup(x => x.GetStudentByRollAsync(roll))
            .ReturnsAsync((Student?)null);

        var request = new GetStudentByPersonalInfoRequest
        {
            Roll = roll
        };

        var handler = new GetStudentByPersonalInfoRequestHandler(
            unitOfWork.Object,
            userService.Object);

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        result.IsSuccessful.ShouldBeFalse();
        result.Status.ShouldBe(HttpStatusCode.NotFound);
        result.Record.ShouldBeNull();

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

        const string email = "student@example.com";

        studentRepo
            .Setup(x => x.GetStudentByEmailAsync(email))
            .ThrowsAsync(new InvalidOperationException("Database failure"));

        var request = new GetStudentByPersonalInfoRequest
        {
            Email = email
        };

        var handler = new GetStudentByPersonalInfoRequestHandler(
            unitOfWork.Object,
            userService.Object);

        // Act
        var act = () => handler.Handle(request, CancellationToken.None);

        // Assert
        await Should.ThrowAsync<FailToProcessQueryException>(act);
    }
}
