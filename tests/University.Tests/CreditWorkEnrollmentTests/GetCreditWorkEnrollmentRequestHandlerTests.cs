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
using University.Application.Exceptions;
using University.Application.Features.CreditWorkEnrollment.Handlers.Queries;
using University.Application.Features.CreditWorkEnrollment.Requests.Requests;
using University.Application.Models.DTOs.Staff;
using University.Tests.Common;
using Entities = University.Domain.Entities;

namespace University.Tests.CreditWorkEnrollmentTests
{
    public class GetCreditWorkEnrollmentRequestHandlerTests
    {
        private readonly Mock<ICreditWorkEnrollmentRepository> _creditWorkEnrollmentRepositoryMock = new();
        private readonly Mock<IUserService> _userServiceMock = new();
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;

        public GetCreditWorkEnrollmentRequestHandlerTests()
        {
            _unitOfWorkMock = UnitOfWorkMock.Create(
                creditWorkEnrollmentRepo: _creditWorkEnrollmentRepositoryMock);
        }

        private GetCreditWorkEnrollmentRequestHandler CreateHandler() =>
            new(_unitOfWorkMock.Object, _userServiceMock.Object);

        private static Entities.JunctionEntities.CreditWorkEnrollment BuildEnrollment(Guid enrollmentId, Guid createdById) =>
            new()
            {
                Id = enrollmentId,
                CreatedById = createdById,
                CreatedAt = DateTimeOffset.UtcNow,
                CreditWork = new Entities.BaseEntities.CreditWork { Id = Guid.NewGuid() },
                Student = new Entities.BaseEntities.Student { UserId = Guid.NewGuid() }
            };

        [Fact]
        public async Task Handle_WhenEnrollmentExists_ShouldReturnOkWithMappedDto()
        {
            // Arrange
            var enrollmentId = Guid.NewGuid();
            var createdById = Guid.NewGuid();
            var staff = new StaffDto();
            var enrollment = BuildEnrollment(enrollmentId, createdById);

            _creditWorkEnrollmentRepositoryMock
                .Setup(r => r.GetEnrollment(enrollmentId))
                .ReturnsAsync(enrollment);
            _userServiceMock
                .Setup(s => s.GetStaffByIdAsync(createdById))
                .ReturnsAsync(staff);

            var request = new GetCreditWorkEnrollmentRequest { CreditWorkEnrollmentId = enrollmentId };
            var handler = CreateHandler();

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            result.IsSuccessful.ShouldBeTrue();
            result.Status.ShouldBe(HttpStatusCode.OK);
            result.Record.ShouldNotBeNull();
            result.Record!.CreatedAt.ShouldBe(enrollment.CreatedAt);
            result.Record.CreatedBy.ShouldBeSameAs(staff);
        }

        [Fact]
        public async Task Handle_WhenEnrollmentDoesNotExist_ShouldReturnNotFoundWithMessage()
        {
            // Arrange
            var enrollmentId = Guid.NewGuid();
            _creditWorkEnrollmentRepositoryMock
                .Setup(r => r.GetEnrollment(enrollmentId))
                .ReturnsAsync((Entities.CreditWorkEnrollment?)null);

            var request = new GetCreditWorkEnrollmentRequest { CreditWorkEnrollmentId = enrollmentId };
            var handler = CreateHandler();

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            result.IsSuccessful.ShouldBeFalse();
            result.Status.ShouldBe(HttpStatusCode.NotFound);
            result.Message.ShouldBe("No credit works found");
            _userServiceMock.Verify(s => s.GetStaffByIdAsync(It.IsAny<Guid>()), Times.Never);
        }

        [Fact]
        public async Task Handle_WhenStaffLookupReturnsNull_ShouldFallBackToDefaultStaffDto()
        {
            // Arrange — covers the `?? new StaffDto()` fallback in the handler
            var enrollmentId = Guid.NewGuid();
            var createdById = Guid.NewGuid();
            var enrollment = BuildEnrollment(enrollmentId, createdById);

            _creditWorkEnrollmentRepositoryMock
                .Setup(r => r.GetEnrollment(enrollmentId))
                .ReturnsAsync(enrollment);
            _userServiceMock
                .Setup(s => s.GetStaffByIdAsync(createdById))
                .ReturnsAsync((StaffDto?)null);

            var request = new GetCreditWorkEnrollmentRequest { CreditWorkEnrollmentId = enrollmentId };
            var handler = CreateHandler();

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            result.IsSuccessful.ShouldBeTrue();
            result.Record!.CreatedBy.ShouldNotBeNull();
        }

        [Fact]
        public async Task Handle_WhenRepositoryThrows_ShouldWrapInFailToProcessQueryException()
        {
            // Arrange
            var enrollmentId = Guid.NewGuid();
            _creditWorkEnrollmentRepositoryMock
                .Setup(r => r.GetEnrollment(enrollmentId))
                .ThrowsAsync(new InvalidOperationException("db unavailable"));

            var request = new GetCreditWorkEnrollmentRequest { CreditWorkEnrollmentId = enrollmentId };
            var handler = CreateHandler();

            // Act
            var act = () => handler.Handle(request, CancellationToken.None);

            // Assert
            await Should.ThrowAsync<FailToProcessQueryException>(act);
        }
    }
}
