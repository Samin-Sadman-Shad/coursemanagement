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
using University.Application.Exceptions;
using University.Application.Features.CreditWork.Handlers.Commands;
using University.Application.Features.CreditWork.Requests.Commands;
using University.Application.Models.DTOs.CreditWorkDTOs;
using University.Application.Utils;
using University.Domain.Entities.BaseEntities;
using University.Tests.Common;

namespace University.Tests.CreditWorkTests
{
    public class UpdateCreditWorkCommandHandlerTests
    {
        [Fact]
        public async Task Handle_ValidCreditWork_UpdatesAndReturnsNoContent()
        {
            // Arrange
            var staffId = Guid.NewGuid();
            var creditWork = EntityTestFactory.CreateCreditWork();

            var dto = new UpdateCreditWorkDto
            {
                Heading = "Database",
                Code = 202,
                Description = "Updated description"
            };

            var repo = new Mock<ICreditWorkRepository>();

            repo.Setup(x => x.DoesCreditWorkTitleExistAsync(dto.Heading, dto.Code, null))
                .ReturnsAsync(false);

            repo.Setup(x => x.GetByIdAsync(creditWork.Id))
                .ReturnsAsync(creditWork);

            var uow = UnitOfWorkMock.Create(creditWorkRepo: repo);
            uow.Setup(x => x.SaveChangesAsync()).Returns(Task.CompletedTask);

            var currentUser = new Mock<ICurrentUserService>();
            currentUser.SetupGet(x => x.UserId).Returns(staffId);

            var handler = new UpdateCreditWorkCommandHandler(
                uow.Object,
                Mock.Of<IUserService>(),
                currentUser.Object);

            var command = new UpdateCreditWorkCommand
            {
                CreditWorkId = creditWork.Id,
                CreditWorkDto = dto
            };

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsSuccessful.ShouldBeTrue();
            result.Status.ShouldBe(HttpStatusCode.NoContent);
            result.RecordId.ShouldBe(creditWork.Id);

            creditWork.Heading.ShouldBe(dto.Heading);
            creditWork.Code.ShouldBe(dto.Code);
            creditWork.Description.ShouldBe(dto.Description);
            creditWork.LastModifiedById.ShouldBe(staffId);

            uow.Verify(x => x.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task Handle_DuplicateCreditWorkTitle_ReturnsBadRequest()
        {
            // Arrange
            var dto = new UpdateCreditWorkDto
            {
                Heading = "Programming",
                Code = 101
            };

            var repo = new Mock<ICreditWorkRepository>();

            repo.Setup(x => x.DoesCreditWorkTitleExistAsync(dto.Heading, dto.Code, null))
                .ReturnsAsync(true);

            var uow = UnitOfWorkMock.Create(creditWorkRepo: repo);

            var handler = new UpdateCreditWorkCommandHandler(
                uow.Object,
                Mock.Of<IUserService>(),
                Mock.Of<ICurrentUserService>());

            var command = new UpdateCreditWorkCommand
            {
                CreditWorkId = Guid.NewGuid(),
                CreditWorkDto = dto
            };

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsSuccessful.ShouldBeFalse();
            result.Status.ShouldBe(HttpStatusCode.BadRequest);
            //result.Errors.ShouldContain("Code update will create duplicate entity");

            repo.Verify(x => x.GetByIdAsync(It.IsAny<Guid>()), Times.Never);
            uow.Verify(x => x.SaveChangesAsync(), Times.Never);
        }

        [Fact]
        public async Task Handle_NonExistingCreditWork_ThrowsFailToProcessCommandException()
        {
            // Arrange
            var creditWorkId = Guid.NewGuid();

            var dto = new UpdateCreditWorkDto
            {
                Heading = "Programming",
                Code = 101
            };

            var repo = new Mock<ICreditWorkRepository>();

            repo.Setup(x => x.DoesCreditWorkTitleExistAsync(dto.Heading, dto.Code, null))
                .ReturnsAsync(false);

            repo.Setup(x => x.GetByIdAsync(creditWorkId))
                .ReturnsAsync((CreditWork?)null);

            var uow = UnitOfWorkMock.Create(creditWorkRepo: repo);

            var handler = new UpdateCreditWorkCommandHandler(
                uow.Object,
                Mock.Of<IUserService>(),
                Mock.Of<ICurrentUserService>());

            var command = new UpdateCreditWorkCommand
            {
                CreditWorkId = creditWorkId,
                CreditWorkDto = dto
            };

            // Act
            var act = () => handler.Handle(command, CancellationToken.None);

            // Assert
            await Should.ThrowAsync<FailToProcessCommandException>(act);
            uow.Verify(x => x.SaveChangesAsync(), Times.Never);
        }
    }
}
