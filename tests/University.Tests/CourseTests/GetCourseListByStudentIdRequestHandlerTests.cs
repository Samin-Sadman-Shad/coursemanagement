using System.Net;
using Moq;
using Shouldly;
using University.Application.Contracts.Identity;
using University.Application.Contracts.Persistance;
using University.Application.Features.Course.Handlers.Queries;
using University.Application.Features.Course.Requests;
using University.Application.Features.Course.Requests.Queries;
using University.Application.Models.DTOs.Staff;
using University.Domain.Entities;
using University.Domain.Entities.BaseEntities;
using University.Tests.Common;
using Xunit;

namespace University.Tests.CourseTests;

public class GetCourseListByStudentIdRequestHandlerTests
{
    [Fact]
    public async Task Handle_ExistingRecords_ReturnsMappedCourses()
    {
        // Arrange
        var staffId = Guid.NewGuid();
        var first = EntityTestFactory.CreateCourse();
        var second = EntityTestFactory.CreateCourse();
        first.CreatedById = staffId;
        second.CreatedById = staffId;

        var courseRepo = new Mock<ICourseRepository>();
        courseRepo.Setup(x => x.GetCoursesByStudentIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(new List<Course> { first, second });

        var uow = UnitOfWorkMock.Create(courseRepo: courseRepo);
        var userService = new Mock<IUserService>();
        userService.Setup(x => x.GetStaffByIdAsync(staffId))
            .ReturnsAsync(new StaffDto());

        var handler = new GetCourseListByStudentIdRequestHandler(uow.Object, userService.Object);
        var request = new GetCourseListByStudentIdRequest { StudentId = Guid.NewGuid() };

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        result.IsSuccessful.ShouldBeTrue();
        result.Status.ShouldBe(HttpStatusCode.OK);
        result.Records.ShouldNotBeNull();
        result.Records.Count.ShouldBe(2);
        result.Records[0].CourseTitle.ShouldBe(first.Title);
        result.Records[1].CourseTitle.ShouldBe(second.Title);
        userService.Verify(x => x.GetStaffByIdAsync(staffId), Times.Exactly(2));
    }

    [Fact]
    public async Task Handle_NoRecords_ReturnsEmptyList()
    {
        // Arrange
        var courseRepo = new Mock<ICourseRepository>();
        courseRepo.Setup(x => x.GetCoursesByStudentIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(new List<Course>());

        var uow = UnitOfWorkMock.Create(courseRepo: courseRepo);
        var handler = new GetCourseListByStudentIdRequestHandler(uow.Object, new Mock<IUserService>().Object);
        var request = new GetCourseListByStudentIdRequest { StudentId = Guid.NewGuid() };

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        result.IsSuccessful.ShouldBeTrue();
        result.Status.ShouldBe(HttpStatusCode.OK);
        result.Records.ShouldBeEmpty();
    }
}
