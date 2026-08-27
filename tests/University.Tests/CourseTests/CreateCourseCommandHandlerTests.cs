using System.Net;
using Moq;
using Shouldly;
using University.Application.Features.Course.Handlers.Commands;
using University.Domain.Entities;
using Xunit;
using University.Application.Models.DTOs.CourseDTOs;
using University.Application.Contracts.Persistance;
using University.Domain.Entities.BaseEntities;
using University.Tests.Common;
using University.Application.Contracts.Identity;
using University.Application.Models.DTOs.Staff;
using University.Application.Features.Course.Requests.Commands;
using University.Application.Contracts.API;
using University.Application.Utils;

namespace University.Tests.CourseTests;

public class CreateCourseCommandHandlerTests
{
    [Fact]
    public async Task Handle_ValidCourse_CreatesCourse()
    {
        // Arrange
        var staffId = Guid.NewGuid();
        var courseId = Guid.NewGuid();
        var dto = new CreateCourseDto { CourseTitle = "Programming101" };

        var courseRepo = new Mock<ICourseRepository>();
        courseRepo.Setup(x => x.DoesCourseNameExistsAsync(dto.CourseTitle))
            .ReturnsAsync(false);
        courseRepo.Setup(x => x.CreateAsync(It.IsAny<Course>()))
            .ReturnsAsync((Course entity) =>
            {
                entity.Id = courseId;
                return entity;
            });

        var uow = UnitOfWorkMock.Create(courseRepo: courseRepo);
        uow.Setup(x => x.SaveChangesAsync()).Returns(Task.CompletedTask);

        var userService = new Mock<IUserService>();
        userService.Setup(x => x.GetStaffByIdAsync(staffId))
            .ReturnsAsync(new StaffDto());

        var currentUser = new Mock<ICurrentUserService>();
        currentUser.SetupGet(x => x.UserId).Returns(staffId);

        var handler = new CreateCourseCommandHandler(
            uow.Object, userService.Object, currentUser.Object);

        var request = new CreateCourseCommand { CreateCourseDto = dto };

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        result.IsSuccessful.ShouldBeTrue();
        result.Status.ShouldBe(HttpStatusCode.Created);
        result.RecordId.ShouldBe(courseId);
        result.Record.ShouldNotBeNull();
        result.Record!.CourseTitle.ShouldBe(dto.CourseTitle);

        courseRepo.Verify(x => x.CreateAsync(It.Is<Course>(c =>
            c.Title == dto.CourseTitle &&
            c.CreatedById == staffId &&
            c.LastModifiedById == staffId)), Times.Once);
        uow.Verify(x => x.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task Handle_DuplicateCourseTitle_ReturnsBadRequest()
    {
        // Arrange
        var dto = new CreateCourseDto { CourseTitle = "Programming101" };
        var courseRepo = new Mock<ICourseRepository>();

        courseRepo.Setup(x => x.DoesCourseNameExistsAsync(dto.CourseTitle))
            .ReturnsAsync(true);

        var uow = UnitOfWorkMock.Create(courseRepo: courseRepo);
        var userService = new Mock<IUserService>();
        var currentUser = new Mock<ICurrentUserService>();

        var handler = new CreateCourseCommandHandler(
            uow.Object, userService.Object, currentUser.Object);

        var request = new CreateCourseCommand { CreateCourseDto = dto };

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        result.IsSuccessful.ShouldBeFalse();
        result.Status.ShouldBe(HttpStatusCode.BadRequest);
        //result.Errors.ShouldContain("Course Title update will create duplicate entity");
        courseRepo.Verify(x => x.CreateAsync(It.IsAny<Course>()), Times.Never);
    }

    [Fact]
    public async Task Handle_InvalidCourseTitle_ReturnsBadRequest()
    {
        // Arrange
        var dto = new CreateCourseDto { CourseTitle = "Invalid Title" };
        var courseRepo = new Mock<ICourseRepository>();
        var uow = UnitOfWorkMock.Create(courseRepo: courseRepo);

        var handler = new CreateCourseCommandHandler(
            uow.Object,
            new Mock<IUserService>().Object,
            new Mock<ICurrentUserService>().Object);

        var request = new CreateCourseCommand { CreateCourseDto = dto };

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        result.IsSuccessful.ShouldBeFalse();
        result.Status.ShouldBe(HttpStatusCode.BadRequest);
        //result.Errors.ShouldContain("The Course Title can contains only alphanumeric characters");
        courseRepo.Verify(x => x.CreateAsync(It.IsAny<Course>()), Times.Never);
    }
}
