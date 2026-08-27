using Moq;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using University.Application.Contracts.Persistance;
using University.Application.Exceptions;
using University.Application.Features.CreditWork.Handlers.Commands;
using University.Application.Features.CreditWork.Requests.Commands;
using University.Domain.Entities.BaseEntities;
using University.Tests.Common;

namespace University.Tests.CreditWorkTests
{
    public class DeleteCreditWorkCommandHandlerTests
    {
        [Fact]
        public async Task Handle_ExistingCreditWork_DeletesAndReturnsNoContent()
        {
            // Arrange
            var creditWork = EntityTestFactory.CreateCreditWork();
            var repo = new Mock<ICreditWorkRepository>();

            repo.Setup(x => x.GetByIdAsync(creditWork.Id))
                .ReturnsAsync(creditWork);

            repo.Setup(x => x.DeleteAsync(creditWork.Id))
                .ReturnsAsync(creditWork);

            var uow = UnitOfWorkMock.Create(creditWorkRepo: repo);
            uow.Setup(x => x.SaveChangesAsync()).Returns(Task.CompletedTask);

            var handler = new DeleteCreditWorkCommandHandler(uow.Object);
            var command = new DeleteCreditWorkCommand { CreditWorkId = creditWork.Id };

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsSuccessful.ShouldBeTrue();
            result.Status.ShouldBe(HttpStatusCode.NoContent);
            result.RecordId.ShouldBe(creditWork.Id);

            repo.Verify(x => x.GetByIdAsync(creditWork.Id), Times.Once);
            repo.Verify(x => x.DeleteAsync(creditWork.Id), Times.Once);
            uow.Verify(x => x.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task Handle_NonExistingCreditWork_ThrowsFailToProcessCommandException()
        {
            // Arrange
            var creditWorkId = Guid.NewGuid();
            var repo = new Mock<ICreditWorkRepository>();

            repo.Setup(x => x.GetByIdAsync(creditWorkId))
                .ReturnsAsync((CreditWork?)null);

            var uow = UnitOfWorkMock.Create(creditWorkRepo: repo);
            var handler = new DeleteCreditWorkCommandHandler(uow.Object);

            var command = new DeleteCreditWorkCommand { CreditWorkId = creditWorkId };

            // Act
            var act = () => handler.Handle(command, CancellationToken.None);

            // Assert
            await Should.ThrowAsync<FailToProcessCommandException>(act);
            repo.Verify(x => x.DeleteAsync(It.IsAny<Guid>()), Times.Never);
            uow.Verify(x => x.SaveChangesAsync(), Times.Never);
        }
    }
}
