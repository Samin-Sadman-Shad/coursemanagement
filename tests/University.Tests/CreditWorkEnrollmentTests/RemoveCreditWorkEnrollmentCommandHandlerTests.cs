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
using University.Application.Features.CreditWorkEnrollment.Handlers.Commands;
using University.Application.Features.CreditWorkEnrollment.Requests.Commands;
using University.Tests.Common;
using Entities = University.Domain.Entities;

namespace University.Tests.CreditWorkEnrollmentTests
{
    //public class RemoveCreditWorkEnrollmentCommandHandlerTests
    //{
    //    private readonly Mock<ICreditWorkEnrollmentRepository> _creditWorkEnrollmentRepositoryMock = new();
    //    private readonly Mock<IUnitOfWork> _unitOfWorkMock;

    //    public RemoveCreditWorkEnrollmentCommandHandlerTests()
    //    {
    //        _unitOfWorkMock = UnitOfWorkMock.Create(
    //            creditWorkEnrollmentRepo: _creditWorkEnrollmentRepositoryMock);
    //    }

    //    private RemoveCreditWorkEnrollmentCommandHandler CreateHandler() => new(_unitOfWorkMock.Object);

    //    [Fact]
    //    public async Task Handle_WhenEnrollmentExists_ShouldRemoveAndReturnNoContent()
    //    {
    //        // Arrange
    //        var enrollmentId = Guid.NewGuid();
    //        var enrollment = new Entities.JunctionEntities.CreditWorkEnrollment
    //        {
    //            Id = enrollmentId,

    //        };

    //        _creditWorkEnrollmentRepositoryMock
    //            .Setup(r => r.GetEnrollment(enrollmentId))
    //            .ReturnsAsync(enrollment);

    //        var command = new RemoveCreditWorkEnrollmentCommand { CreditWorkEnrollmentId = enrollmentId };
    //        var handler = CreateHandler();

    //        // Act
    //        var result = await handler.Handle(command, CancellationToken.None);

    //        // Assert
    //        result.IsSuccessful.ShouldBeTrue();
    //        result.Status.ShouldBe(HttpStatusCode.NoContent);
    //        result.RecordId.ShouldBe(enrollmentId);
    //        _creditWorkEnrollmentRepositoryMock.Verify(r => r.RemoveCreditWorkEnrollment(enrollment), Times.Once);
    //        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Once);
    //    }

    //    [Fact]
    //    public async Task Handle_WhenEnrollmentDoesNotExist_ShouldReturnNotFoundAndNotSave()
    //    {
    //        // Arrange
    //        var enrollmentId = Guid.NewGuid();
    //        _creditWorkEnrollmentRepositoryMock
    //            .Setup(r => r.GetEnrollment(enrollmentId))
    //            .ReturnsAsync((Entities.CreditWorkEnrollment?)null);

    //        var command = new RemoveCreditWorkEnrollmentCommand { CreditWorkEnrollmentId = enrollmentId };
    //        var handler = CreateHandler();

    //        // Act
    //        var result = await handler.Handle(command, CancellationToken.None);

    //        // Assert
    //        result.IsSuccessful.ShouldBeFalse();
    //        result.Status.ShouldBe(HttpStatusCode.NotFound);
    //        _creditWorkEnrollmentRepositoryMock.Verify(
    //            r => r.RemoveCreditWorkEnrollment(It.IsAny<Entities.CreditWorkEnrollment>()), Times.Never);
    //        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Never);
    //    }

    //    [Fact]
    //    public async Task Handle_WhenRepositoryThrows_ShouldWrapInFailToProcessCommandException()
    //    {
    //        // Arrange
    //        var enrollmentId = Guid.NewGuid();
    //        _creditWorkEnrollmentRepositoryMock
    //            .Setup(r => r.GetEnrollment(enrollmentId))
    //            .ThrowsAsync(new InvalidOperationException("db unavailable"));

    //        var command = new RemoveCreditWorkEnrollmentCommand { CreditWorkEnrollmentId = enrollmentId };
    //        var handler = CreateHandler();

    //        // Act
    //        var act = () => handler.Handle(command, CancellationToken.None);

    //        // Assert
    //        await Should.ThrowAsync<FailToProcessCommandException>(act);
    //    }
    //}
}
