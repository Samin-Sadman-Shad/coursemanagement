using Moq;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using University.Application.Contracts.Persistance;
using University.Application.Features.CourseEnrollment.Handlers.Commands;
using University.Application.Features.CourseEnrollment.Requests.Commands;
using University.Domain.Entities.JunctionEntities;
using University.Tests.Common;

namespace University.Tests.CourseEnrollmentTests
{
    public class RemoveCourseEnrollmentCommandHandlerTests
    {
        [Fact]
        public async Task Handle_ShouldReturnNotFound_WhenEnrollmentDoesNotExist()
        {
            var uow = new Mock<IUnitOfWork>();
            var repo = new Mock<ICourseEnrollmentRepository>();
            var id = Guid.NewGuid();

            uow.SetupGet(x => x.CourseEnrollmentRepository).Returns(repo.Object);
            repo.Setup(x => x.GetEnrollment(id))
                .ReturnsAsync((CourseEnrollment?)null);

            var request = new RemoveCourseEnrollmentCommand
            {
                CourseEnrollmentId = id
            };

            var handler = new RemoveCourseEnrollmentCommandHandler(uow.Object);

            var result = await handler.Handle(request, CancellationToken.None);

            result.IsSuccessful.ShouldBeFalse();
            result.Status.ShouldBe(HttpStatusCode.NotFound);

            uow.Verify(x => x.SaveChangesAsync(), Times.Never);
        }

        [Fact]
        public async Task Handle_ShouldRemoveCourseEnrollment()
        {
            var uow = new Mock<IUnitOfWork>();
            var repo = new Mock<ICourseEnrollmentRepository>();
            var id = Guid.NewGuid();

            var enrollment = EntityTestFactory.CreateCourseEnrollment(id);

            uow.SetupGet(x => x.CourseEnrollmentRepository).Returns(repo.Object);
            repo.Setup(x => x.GetEnrollment(id)).ReturnsAsync(enrollment);

            var request = new RemoveCourseEnrollmentCommand
            {
                CourseEnrollmentId = id
            };

            var handler = new RemoveCourseEnrollmentCommandHandler(uow.Object);

            var result = await handler.Handle(request, CancellationToken.None);

            result.IsSuccessful.ShouldBeTrue();
            result.Status.ShouldBe(HttpStatusCode.NoContent);
            result.RecordId.ShouldBe(id);

            repo.Verify(x => x.RemoveCourseEnrollment(enrollment), Times.Once);
            uow.Verify(x => x.SaveChangesAsync(), Times.Once);
        }
    }
}
