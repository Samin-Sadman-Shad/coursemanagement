using Moq;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using University.Application.Contracts.Identity;
using University.Application.Contracts.Persistance;
using University.Application.Features.CreditWork.Handlers.Queries;
using University.Application.Features.CreditWork.Requests.Queries;
using University.Application.Models.DTOs.Staff;
using University.Domain.Entities.BaseEntities;
using University.Tests.Common;

namespace University.Tests.CreditWorkTests
{
    public class GetCreditWorksByCourseIdRequestHandlerTests
    {
        [Fact]
        public async Task Handle_CreditWorksExist_ReturnsMappedRecords()
        {
            // Arrange
            var courseId = Guid.NewGuid();
            var creditWork1 = EntityTestFactory.CreateCreditWork();
            var creditWork2 = EntityTestFactory.CreateCreditWork();

            var repo = new Mock<ICreditWorkRepository>();

            repo.Setup(x => x.GetCreditWorksByCourseIdAsync(courseId))
                .ReturnsAsync(new List<CreditWork> { creditWork1, creditWork2 });

            var uow = UnitOfWorkMock.Create(creditWorkRepo: repo);

            var handler = new GetCreditWorksByCourseIdRequestHandler(
                uow.Object, Mock.Of<IUserService>());

            var request = new GetCreditWorksByCourseIdRequest
            {
                CourseId = courseId
            };

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            result.IsSuccessful.ShouldBeTrue();
            result.Status.ShouldBe(HttpStatusCode.OK);
            result.Records.Count.ShouldBe(2);
            result.Records[0].Id.ShouldBe(creditWork1.Id);
            result.Records[1].Id.ShouldBe(creditWork2.Id);
        }

        [Fact]
        public async Task Handle_NoCreditWorks_ReturnsNotFound()
        {
            // Arrange
            var courseId = Guid.NewGuid();

            var repo = new Mock<ICreditWorkRepository>();

            repo.Setup(x => x.GetCreditWorksByCourseIdAsync(courseId))
                .ReturnsAsync((List<CreditWork>)null!);

            var uow = UnitOfWorkMock.Create(creditWorkRepo: repo);

            var handler = new GetCreditWorksByCourseIdRequestHandler(
                uow.Object, Mock.Of<IUserService>());

            // Act
            var result = await handler.Handle(
                new GetCreditWorksByCourseIdRequest { CourseId = courseId },
                CancellationToken.None);

            // Assert
            result.IsSuccessful.ShouldBeFalse();
            result.Status.ShouldBe(HttpStatusCode.NotFound);
            result.Message.ShouldBe("No credit works found");
        }

        [Fact]
        public async Task Handle_StaffNotFound_UsesEmptyStaffDto()
        {
            // Arrange
            var creditWork = EntityTestFactory.CreateCreditWork();

            var repo = new Mock<ICreditWorkRepository>();

            repo.Setup(x => x.GetCreditWorksByCourseIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(new List<CreditWork> { creditWork });

            var userService = new Mock<IUserService>();

            userService.Setup(x => x.GetStaffByIdAsync(creditWork.CreatedById))
                .ReturnsAsync((StaffDto?)null);

            var uow = UnitOfWorkMock.Create(creditWorkRepo: repo);

            var handler = new GetCreditWorksByCourseIdRequestHandler(
                uow.Object, userService.Object);

            // Act
            var result = await handler.Handle(
                new GetCreditWorksByCourseIdRequest { CourseId = Guid.NewGuid() },
                CancellationToken.None);

            // Assert
            result.IsSuccessful.ShouldBeTrue();
            result.Records.Count.ShouldBe(1);
            result.Records[0].CreatedBy.ShouldNotBeNull();
        }
    }
}
