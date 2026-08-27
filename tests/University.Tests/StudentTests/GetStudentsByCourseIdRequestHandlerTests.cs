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

public class GetStudentsByCourseIdRequestHandlerTests
{
    [Fact]
    public async Task Handle_StudentsExistForCourse_ReturnsMappedStudents()
    {
        // Arrange
        var studentRepo = new Mock<IStudentRepository>();
        var unitOfWork = UnitOfWorkMock.Create(studentRepo);
        var userService = new Mock<IUserService>();

        var courseId = Guid.NewGuid();

        var student1 = EntityTestFactory.CreateStudent();
        student1.Name = "Student One";
        student1.Roll = 1;
        student1.CreatedById = Guid.NewGuid();

        var student2 = EntityTestFactory.CreateStudent();
        student2.Name = "Student Two";
        student2.Roll = 2;
        student2.CreatedById = Guid.NewGuid();

        studentRepo
            .Setup(x => x.GetStudentsByCourseIdAsync(courseId))
            .ReturnsAsync(new List<Student> { student1, student2 });

        userService
            .Setup(x => x.GetStaffByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(new StaffDto());

        var request = new GetStudentsByCourseIdRequest
        {
            CourseId = courseId
        };

        var handler = new GetStudentsByCourseIdRequestHandler(
            unitOfWork.Object,
            userService.Object);

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        result.IsSuccessful.ShouldBeTrue();
        result.Status.ShouldBe(HttpStatusCode.OK);
        result.Records.Count.ShouldBe(2);
        result.Records[0].Name.ShouldBe("Student One");
        result.Records[1].Name.ShouldBe("Student Two");

        studentRepo.Verify(x => x.GetStudentsByCourseIdAsync(courseId), Times.Once);
        userService.Verify(x => x.GetStaffByIdAsync(It.IsAny<Guid>()), Times.Exactly(2));
    }

    [Fact]
    public async Task Handle_NoStudentsForCourse_ReturnsEmptyList()
    {
        // Arrange
        var studentRepo = new Mock<IStudentRepository>();
        var unitOfWork = UnitOfWorkMock.Create(studentRepo);
        var userService = new Mock<IUserService>();

        var courseId = Guid.NewGuid();

        studentRepo
            .Setup(x => x.GetStudentsByCourseIdAsync(courseId))
            .ReturnsAsync(new List<Student>());

        var request = new GetStudentsByCourseIdRequest
        {
            CourseId = courseId
        };

        var handler = new GetStudentsByCourseIdRequestHandler(
            unitOfWork.Object,
            userService.Object);

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        result.IsSuccessful.ShouldBeTrue();
        result.Status.ShouldBe(HttpStatusCode.OK);
        result.Records.ShouldNotBeNull();
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

        var courseId = Guid.NewGuid();

        studentRepo
            .Setup(x => x.GetStudentsByCourseIdAsync(courseId))
            .ThrowsAsync(new InvalidOperationException("Database failure"));

        var request = new GetStudentsByCourseIdRequest
        {
            CourseId = courseId
        };

        var handler = new GetStudentsByCourseIdRequestHandler(
            unitOfWork.Object,
            userService.Object);

        // Act
        var act = () => handler.Handle(request, CancellationToken.None);

        // Assert
        await Should.ThrowAsync<FailToProcessQueryException>(act);
    }
}
