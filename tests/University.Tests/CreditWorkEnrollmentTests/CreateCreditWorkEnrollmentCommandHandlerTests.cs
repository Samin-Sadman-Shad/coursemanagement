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
using University.Application.Features.CreditWorkEnrollment.Handlers.Commands;
using University.Application.Features.CreditWorkEnrollment.Requests.Commands;
using University.Application.Models.DTOs.CreditWorkEnrollmentDto;
using University.Application.Models.DTOs.Staff;
using University.Domain.Entities.BaseEntities;
using University.Domain.Entities.JunctionEntities;
using University.Tests.Common;

namespace University.Tests.CreditWorkEnrollmentTests
{
    public class CreateCreditWorkEnrollmentCommandHandlerTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<ICurrentUserService> _currentUserServiceMock = new();
        private readonly Mock<IUserService> _userServiceMock=new();

        private readonly Mock<IStudentRepository> _studentRepoMock = new();
        private readonly Mock<ICreditWorkRepository> _creditWorkRepoMock = new();
        private readonly Mock<ICreditWorkEnrollmentRepository> _creditWorkEnrollRepoMock = new();

        private readonly Guid _userId = Guid.NewGuid();

        public CreateCreditWorkEnrollmentCommandHandlerTests()
        {
            _unitOfWorkMock = UnitOfWorkMock.Create(
                studentRepo: _studentRepoMock,
                creditWorkRepo: _creditWorkRepoMock,
                creditWorkEnrollmentRepo: _creditWorkEnrollRepoMock);

            _currentUserServiceMock.Setup(service => service.UserId).Returns(_userId);
        }

        private CreateCreditWorkEnrollmentCommandHandler CreateHandler()
        {
            return new CreateCreditWorkEnrollmentCommandHandler(_unitOfWorkMock.Object,
                _currentUserServiceMock.Object,
                _userServiceMock.Object);
        }

        private void ArrangeValidationPasses(Guid studentId, Guid creditWorkId)
        {
            _creditWorkRepoMock.Setup(r => r.ExistsAsync(creditWorkId)).ReturnsAsync(true);
            _studentRepoMock.Setup(r => r.ExistsAsync(studentId)).ReturnsAsync(true);
            _creditWorkEnrollRepoMock
                .Setup(r => r.ExistsAsync(studentId, creditWorkId))
                .ReturnsAsync(false); // not already enrolled
        }

        private static CreateCreditWorkEnrollmentCommand CreateCommand(Guid studentId, Guid creditWorkId) => new()
        {
            CreditWorkEnrollmentDto = new CreateCreditWorkEnrollmentDto
            {
                StudentId = studentId,
                CreditWorkId = creditWorkId
            }
        };

        [Fact]
        public async Task Handle_WhenValidationPassesAndEntitiesExist_ShouldCreateEnrollmentAndReturnCreated()
        {
            // Arrange
            var studentId = Guid.NewGuid();
            var creditWorkId = Guid.NewGuid();
            var createdEnrollmentId = Guid.NewGuid();

            ArrangeValidationPasses(studentId, creditWorkId);

            _studentRepoMock.Setup(r => r.GetByIdAsync(studentId))
                .ReturnsAsync(new Student { UserId = studentId });
            _creditWorkRepoMock.Setup(r => r.GetByIdAsync(creditWorkId))
                .ReturnsAsync(new CreditWork { Id = creditWorkId });
            _creditWorkEnrollRepoMock
                .Setup(r => r.CreateCreditWorkEnrollment(It.IsAny<CreditWorkEnrollment>()))
                .ReturnsAsync((CreditWorkEnrollment enrollment) =>
                {
                    enrollment.Id = createdEnrollmentId;
                    return enrollment;
                });
            _userServiceMock.Setup(s => s.GetStaffByIdAsync(_userId)).ReturnsAsync(new StaffDto());

            var command = CreateCommand(studentId, creditWorkId);
            var handler = CreateHandler();

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsSuccessful.ShouldBeTrue();
            result.Status.ShouldBe(HttpStatusCode.Created);
            result.RecordId.ShouldBe(createdEnrollmentId);
            _creditWorkEnrollRepoMock.Verify(
                r => r.CreateCreditWorkEnrollment(It.Is<CreditWorkEnrollment>(e =>
                    e.StudentId == studentId &&
                    e.CreditWorkId == creditWorkId &&
                    e.EnrolledById == _userId &&
                    e.CreatedById == _userId)),
                Times.Once);
            _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task Handle_WhenCreditWorkDoesNotExistPerValidator_ShouldReturnBadRequestAndNotCreateEnrollment()
        {
            // Arrange
            var studentId = Guid.NewGuid();
            var creditWorkId = Guid.NewGuid();

            ArrangeValidationPasses(studentId, creditWorkId);
            _creditWorkRepoMock.Setup(r => r.ExistsAsync(creditWorkId)).ReturnsAsync(false);

            var command = CreateCommand(studentId, creditWorkId);
            var handler = CreateHandler();

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsSuccessful.ShouldBeFalse();
            result.Status.ShouldBe(HttpStatusCode.BadRequest);
            result.Errors.ShouldContain("CreditWork not found");
            _creditWorkRepoMock.Verify(r => r.GetByIdAsync(It.IsAny<Guid>()), Times.Never);
            _creditWorkEnrollRepoMock.Verify(
                r => r.CreateCreditWorkEnrollment(It.IsAny<CreditWorkEnrollment>()), Times.Never);
        }

        [Fact]
        public async Task Handle_WhenStudentDoesNotExistPerValidator_ShouldReturnBadRequestAndNotCreateEnrollment()
        {
            // Arrange
            var studentId = Guid.NewGuid();
            var creditWorkId = Guid.NewGuid();

            ArrangeValidationPasses(studentId, creditWorkId);
            _studentRepoMock.Setup(r => r.ExistsAsync(studentId)).ReturnsAsync(false);

            var command = CreateCommand(studentId, creditWorkId);
            var handler = CreateHandler();

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsSuccessful.ShouldBeFalse();
            result.Status.ShouldBe(HttpStatusCode.BadRequest);
            result.Errors.ShouldContain("Student not found");
            _creditWorkEnrollRepoMock.Verify(
                r => r.CreateCreditWorkEnrollment(It.IsAny<CreditWorkEnrollment>()), Times.Never);
        }

        [Fact]
        public async Task Handle_WhenStudentAlreadyEnrolled_ShouldReturnBadRequestAndNotCreateEnrollment()
        {
            // Arrange
            var studentId = Guid.NewGuid();
            var creditWorkId = Guid.NewGuid();

            ArrangeValidationPasses(studentId, creditWorkId);
            _creditWorkEnrollRepoMock
                .Setup(r => r.ExistsAsync(studentId, creditWorkId))
                .ReturnsAsync(true); // already enrolled

            var command = CreateCommand(studentId, creditWorkId);
            var handler = CreateHandler();

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsSuccessful.ShouldBeFalse();
            result.Status.ShouldBe(HttpStatusCode.BadRequest);
            result.Errors.ShouldContain("Student is already enrolled in this credit work.");
            _creditWorkEnrollRepoMock.Verify(
                r => r.CreateCreditWorkEnrollment(It.IsAny<Entities.CreditWorkEnrollment>()), Times.Never);
        }

        [Fact]
        public async Task Handle_WhenStaffLookupReturnsNull_ShouldStillCreateEnrollmentWithDefaultStaff()
        {
            // Arrange - covers the `?? new StaffDto()` fallback in the handler
            var studentId = Guid.NewGuid();
            var creditWorkId = Guid.NewGuid();

            ArrangeValidationPasses(studentId, creditWorkId);
            _studentRepoMock.Setup(r => r.GetByIdAsync(studentId))
                .ReturnsAsync(new Student { UserId = studentId });
            _creditWorkRepoMock.Setup(r => r.GetByIdAsync(creditWorkId))
                .ReturnsAsync(new CreditWork { Id = creditWorkId });
            _creditWorkEnrollRepoMock
                .Setup(r => r.CreateCreditWorkEnrollment(It.IsAny<CreditWorkEnrollment>()))
                .ReturnsAsync((CreditWorkEnrollment enrollment) => enrollment);
            _userServiceMock.Setup(s => s.GetStaffByIdAsync(_userId)).ReturnsAsync((StaffDto?)null);

            var command = CreateCommand(studentId, creditWorkId);
            var handler = CreateHandler();

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsSuccessful.ShouldBeTrue();
            result.Status.ShouldBe(HttpStatusCode.Created);
        }

        [Fact]
        public async Task Handle_WhenRepositoryThrowsDuringValidation_ShouldWrapInFailToProcessCommandException()
        {
            // Arrange - the throw happens inside the validator's MustAsync call,
            // which is still inside the handler's try block.
            var studentId = Guid.NewGuid();
            var creditWorkId = Guid.NewGuid();

            _creditWorkRepoMock.Setup(r => r.ExistsAsync(creditWorkId))
                .ThrowsAsync(new InvalidOperationException("db unavailable"));

            var command = CreateCommand(studentId, creditWorkId);
            var handler = CreateHandler();

            // Act
            var act = () => handler.Handle(command, CancellationToken.None);

            // Assert
            await Should.ThrowAsync<FailToProcessCommandException>(act);
        }


    }
}
