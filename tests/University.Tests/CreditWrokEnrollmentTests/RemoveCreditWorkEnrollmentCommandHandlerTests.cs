using Moq;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using University.Application.Contracts.Persistance;
using University.Application.Features.CreditWorkEnrollment.Handlers.Commands;
using University.Application.Features.CreditWorkEnrollment.Requests.Commands;
using University.Domain.Entities.JunctionEntities;
using University.Tests.Common;

namespace University.Tests.CreditWrokEnrollmentTests
{
    public class RemoveCreditWorkEnrollmentCommandHandlerTests
    {
        [Fact]
        public async Task Handle_ShouldReturnNotFound_WhenEnrollmentDoesNotExist()
        {
            var uow = new Mock<IUnitOfWork>();
            var repo = new Mock<ICreditWorkEnrollmentRepository>();
            var id = Guid.NewGuid();

            uow.SetupGet(x => x.CreditWorkEnrollmentRepository).Returns(repo.Object);
            repo.Setup(x => x.GetEnrollment(id))
                .ReturnsAsync((CreditWorkEnrollment?)null);

            var request = new RemoveCreditWorkEnrollmentCommand
            {
                CreditWorkEnrollmentId = id
            };

            var handler = new RemoveCreditWorkEnrollmentCommandHandler(uow.Object);

            var result = await handler.Handle(request, CancellationToken.None);

            result.IsSuccessful.ShouldBeFalse();
            result.Status.ShouldBe(HttpStatusCode.NotFound);

            uow.Verify(x => x.SaveChangesAsync(), Times.Never);
        }

        [Fact]
        public async Task Handle_ShouldRemoveCreditWorkEnrollment()
        {
            var uow = new Mock<IUnitOfWork>();
            var repo = new Mock<ICreditWorkEnrollmentRepository>();
            var id = Guid.NewGuid();

            var enrollment = EntityTestFactory.CreateCreditWorkEnrollment(id);

            uow.SetupGet(x => x.CreditWorkEnrollmentRepository).Returns(repo.Object);
            repo.Setup(x => x.GetEnrollment(id)).ReturnsAsync(enrollment);

            var request = new RemoveCreditWorkEnrollmentCommand
            {
                CreditWorkEnrollmentId = id
            };

            var handler = new RemoveCreditWorkEnrollmentCommandHandler(uow.Object);

            var result = await handler.Handle(request, CancellationToken.None);

            result.IsSuccessful.ShouldBeTrue();
            result.Status.ShouldBe(HttpStatusCode.NoContent);
            result.RecordId.ShouldBe(id);

            repo.Verify(x => x.RemoveCreditWorkEnrollment(enrollment), Times.Once);
            uow.Verify(x => x.SaveChangesAsync(), Times.Once);
        }
    }
}
