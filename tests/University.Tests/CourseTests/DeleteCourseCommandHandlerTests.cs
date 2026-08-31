using System.Net;
using Moq;
using Shouldly;
using University.Application.Contracts.Persistance;
using University.Application.Exceptions;
using University.Application.Features.Course.Handlers.Commands;
using University.Application.Features.Course.Requests.Commands;
using University.Domain.Entities;
using University.Domain.Entities.BaseEntities;
using University.Tests.Common;
using Xunit;

namespace University.Tests.CourseTests;

public class DeleteCourseCommandHandlerTests
{
    [Fact]
    public async Task Handle_ExistingCourse_DeletesCourse()
    {
        // Arrange
        var courseId = Guid.NewGuid();
        var course = EntityTestFactory.CreateCourse(courseId);

        var courseRepo = new Mock<ICourseRepository>();
        courseRepo.Setup(x => x.GetByIdAsync(courseId, It.IsAny<CancellationToken>())).ReturnsAsync(course);
        courseRepo.Setup(x => x.DeleteAsync(courseId)).ReturnsAsync(course);

        var uow = UnitOfWorkMock.Create(courseRepo: courseRepo);
        uow.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

        var handler = new DeleteCourseCommandHandler(uow.Object);
        var request = new DeleteCourseCommand { CourseId = courseId };

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        result.IsSuccessful.ShouldBeTrue();
        result.Status.ShouldBe(HttpStatusCode.NoContent);
        result.RecordId.ShouldBe(courseId);

        courseRepo.Verify(x => x.GetByIdAsync(courseId, It.IsAny<CancellationToken>()), Times.Once);
        courseRepo.Verify(x => x.DeleteAsync(courseId), Times.Once);
        uow.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_NonExistingCourse_ThrowsFailToProcessCommandException()
    {
        // Arrange
        var courseId = Guid.NewGuid();
        var courseRepo = new Mock<ICourseRepository>();

        courseRepo.Setup(x => x.GetByIdAsync(courseId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Course?)null);

        var uow = UnitOfWorkMock.Create(courseRepo: courseRepo);
        var handler = new DeleteCourseCommandHandler(uow.Object);
        var request = new DeleteCourseCommand { CourseId = courseId };

        // Act
        var act = () => handler.Handle(request, CancellationToken.None);

        // Assert
        await Should.ThrowAsync<FailToProcessCommandException>(act);
        courseRepo.Verify(x => x.DeleteAsync(It.IsAny<Guid>()), Times.Never);
        uow.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }
}
