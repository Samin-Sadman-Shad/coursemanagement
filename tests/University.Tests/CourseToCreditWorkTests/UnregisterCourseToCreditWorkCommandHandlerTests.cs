using Moq;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using University.Application.Contracts.Persistance;
using University.Application.Features.CourseCreditWorkRegistration.Handlers.Commands;
using University.Application.Features.CourseCreditWorkRegistration.Requests.Commands;
using University.Domain.Entities.JunctionEntities;
using University.Tests.Common;

namespace University.Tests.CourseToCreditWorkTests
{
    public class UnregisterCourseToCreditWorkCommandHandlerTests
    {
        [Fact]
        public async Task Handle_ShouldReturnNotFound_WhenRegistrationDoesNotExist()
        {
            var uow = new Mock<IUnitOfWork>();
            var repo = new Mock<ICourseCreditWorkRegistrationRepository>();
            var id = Guid.NewGuid();

            uow.SetupGet(x => x.CourseCreditWorkRegistrationRepository).Returns(repo.Object);
            repo.Setup(x => x.GetByIdAsync(id)).ReturnsAsync((CourseCreditWork?)null);

            var request = new UnregisterCourseToCreditWorkCommand
            {
                CourseCreditWorkId = id
            };

            var handler = new UnregisterCourseToCreditWorkCommandHandler(uow.Object);

            var result = await handler.Handle(request, CancellationToken.None);

            result.IsSuccessful.ShouldBeFalse();
            result.Status.ShouldBe(HttpStatusCode.NotFound);
            result.RecordId.ShouldBe(id);

            uow.Verify(x => x.SaveChangesAsync(), Times.Never);
        }

        [Fact]
        public async Task Handle_ShouldUnregisterRegistration()
        {
            var uow = new Mock<IUnitOfWork>();
            var repo = new Mock<ICourseCreditWorkRegistrationRepository>();
            var id = Guid.NewGuid();

            var registration = EntityTestFactory.CreateCourseCreditWork();


            uow.SetupGet(x => x.CourseCreditWorkRegistrationRepository).Returns(repo.Object);
            repo.Setup(x => x.GetByIdAsync(id)).ReturnsAsync(registration);

            var request = new UnregisterCourseToCreditWorkCommand
            {
                CourseCreditWorkId = id
            };

            var handler = new UnregisterCourseToCreditWorkCommandHandler(uow.Object);

            var result = await handler.Handle(request, CancellationToken.None);

            result.IsSuccessful.ShouldBeTrue();
            result.Status.ShouldBe(HttpStatusCode.NoContent);
            result.RecordId.ShouldBe(id);

            repo.Verify(x => x.UnregisterCourseFromCreditWork(registration), Times.Once);
            uow.Verify(x => x.SaveChangesAsync(), Times.Once);
        }
    }
}
