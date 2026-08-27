using Moq;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using University.Application.Contracts.API;
using University.Application.Contracts.Identity;
using University.Application.Contracts.Persistance;
using University.Application.Features.CreditWork.Handlers.Commands;
using University.Application.Features.CreditWork.Requests.Commands;
using University.Application.Models.DTOs.CreditWorkDTOs;
using University.Application.Models.DTOs.Staff;
using University.Domain.Entities.BaseEntities;
using University.Tests.Common;
using University.Application.Utils;

namespace University.Tests.CreditWorkTests
{
    public class CreateCreditWorkCommandHandlerTests
    {
        [Fact]
        public async Task Handle_ValidCreditWork_ReturnsCreatedResponse()
        {
            // Arrange
            var staffId = Guid.NewGuid();
            var creditWork = EntityTestFactory.CreateCreditWork();
            var dto = new CreateCreditWorkDto
            {
                Heading = "Programming",
                Code = 101,
                Description = "Introductory course"
            };

            var repo = new Mock<ICreditWorkRepository>();
            repo.Setup(x => x.DoesCreditWorkTitleExistAsync(dto.Heading, dto.Code, null))
                .ReturnsAsync(false);
            repo.Setup(x => x.CreateAsync(It.IsAny<CreditWork>()))
                .ReturnsAsync(creditWork);

            var uow = UnitOfWorkMock.Create(creditWorkRepo: repo);
            uow.Setup(x => x.SaveChangesAsync()).Returns(Task.CompletedTask);

            var userService = new Mock<IUserService>();
            userService.Setup(x => x.GetStaffByIdAsync(staffId))
                .ReturnsAsync(new StaffDto());

            var currentUser = new Mock<ICurrentUserService>();
            currentUser.SetupGet(x => x.UserId).Returns(staffId);

            var handler = new CreateCreditWorkCommandHandler(
                uow.Object, userService.Object, currentUser.Object);

            var command = new CreateCreditWorkCommand { CreateCreditWorkDto = dto };

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsSuccessful.ShouldBeTrue();
            result.Status.ShouldBe(HttpStatusCode.Created);
            result.RecordId.ShouldBe(creditWork.Id);
            result.Record.ShouldNotBeNull();

            repo.Verify(x => x.CreateAsync(It.Is<CreditWork>(e =>
                e.Heading == dto.Heading &&
                e.Code == dto.Code &&
                e.Description == dto.Description &&
                e.CreatedById == staffId)), Times.Once);

            uow.Verify(x => x.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task Handle_DuplicateCreditWorkTitle_ReturnsBadRequest()
        {
            // Arrange
            var dto = new CreateCreditWorkDto
            {
                Heading = "Programming",
                Code = 101
            };

            var repo = new Mock<ICreditWorkRepository>();
            repo.Setup(x => x.DoesCreditWorkTitleExistAsync(dto.Heading, dto.Code, null))
                .ReturnsAsync(true);

            var uow = UnitOfWorkMock.Create(creditWorkRepo: repo);

            var handler = new CreateCreditWorkCommandHandler(
                uow.Object,
                Mock.Of<IUserService>(),
                Mock.Of<ICurrentUserService>());

            var command = new CreateCreditWorkCommand { CreateCreditWorkDto = dto };

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsSuccessful.ShouldBeFalse();
            result.Status.ShouldBe(HttpStatusCode.BadRequest);
            //result.Errors.ShouldContain("Code update will create duplicate entity");

            repo.Verify(x => x.CreateAsync(It.IsAny<CreditWork>()), Times.Never);
            uow.Verify(x => x.SaveChangesAsync(), Times.Never);
        }

        [Fact]
        public async Task Handle_InvalidHeading_ReturnsBadRequest()
        {
            // Arrange
            var dto = new CreateCreditWorkDto
            {
                Heading = "Programming 101",
                Code = 101
            };

            var repo = new Mock<ICreditWorkRepository>();
            var uow = UnitOfWorkMock.Create(creditWorkRepo: repo);

            var handler = new CreateCreditWorkCommandHandler(
                uow.Object,
                Mock.Of<IUserService>(),
                Mock.Of<ICurrentUserService>());

            var command = new CreateCreditWorkCommand { CreateCreditWorkDto = dto };

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsSuccessful.ShouldBeFalse();
            result.Status.ShouldBe(HttpStatusCode.BadRequest);
            //result.Errors.ShouldContain("Heading can contains only letters");

            repo.Verify(x => x.CreateAsync(It.IsAny<CreditWork>()), Times.Never);
        }
    }
}
