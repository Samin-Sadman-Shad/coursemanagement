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
    public class GetCreditWorkListRequestHandlerTests
    {
        [Fact]
        public async Task Handle_CreditWorksExist_ReturnsMappedRecords()
        {
            // Arrange
            var creditWork1 = EntityTestFactory.CreateCreditWork("Math");
            var creditWork2 = EntityTestFactory.CreateCreditWork("Python");

            var repo = new Mock<ICreditWorkRepository>();

            repo.Setup(x => x.GetAllAsync())
                .ReturnsAsync(new List<CreditWork> { creditWork1, creditWork2 });

            var userService = new Mock<IUserService>();

            userService.Setup(x => x.GetStaffByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(new StaffDto());

            var uow = UnitOfWorkMock.Create(creditWorkRepo: repo);

            var handler = new GetCreditWorkListRequestHandler(
                uow.Object, userService.Object);

            // Act
            var result = await handler.Handle(
                new GetCreditWorkListRequest(), CancellationToken.None);

            // Assert
            result.IsSuccessful.ShouldBeTrue();
            result.Status.ShouldBe(HttpStatusCode.OK);
            result.Records.Count.ShouldBe(2);
            result.Records[0].Id.ShouldBe(creditWork1.Id);
            result.Records[1].Id.ShouldBe(creditWork2.Id);

            repo.Verify(x => x.GetAllAsync(), Times.Once);
        }

        [Fact]
        public async Task Handle_RepositoryReturnsNull_ReturnsNotFound()
        {
            // Arrange
            var repo = new Mock<ICreditWorkRepository>();

            repo.Setup(x => x.GetAllAsync())
                .ReturnsAsync((List<CreditWork>)null!);

            var uow = UnitOfWorkMock.Create(creditWorkRepo: repo);

            var handler = new GetCreditWorkListRequestHandler(
                uow.Object, Mock.Of<IUserService>());

            // Act
            var result = await handler.Handle(
                new GetCreditWorkListRequest(), CancellationToken.None);

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

            repo.Setup(x => x.GetAllAsync())
                .ReturnsAsync(new List<CreditWork> { creditWork });

            var userService = new Mock<IUserService>();

            userService.Setup(x => x.GetStaffByIdAsync(creditWork.CreatedById))
                .ReturnsAsync((StaffDto?)null);

            var uow = UnitOfWorkMock.Create(creditWorkRepo: repo);

            var handler = new GetCreditWorkListRequestHandler(
                uow.Object, userService.Object);

            // Act
            var result = await handler.Handle(
                new GetCreditWorkListRequest(), CancellationToken.None);

            // Assert
            result.IsSuccessful.ShouldBeTrue();
            result.Records.Count.ShouldBe(1);
            result.Records[0].CreatedBy.ShouldNotBeNull();
        }
    }
}
