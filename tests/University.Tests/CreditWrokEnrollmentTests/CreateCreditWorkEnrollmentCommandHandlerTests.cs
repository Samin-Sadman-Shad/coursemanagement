using Moq;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using University.Application.Contracts.API;
using University.Application.Contracts.Identity;
using University.Application.Contracts.Persistance;
using University.Application.Features.CreditWorkEnrollment.Handlers.Commands;
using University.Application.Features.CreditWorkEnrollment.Requests.Commands;
using University.Application.Models.DTOs.CreditWorkEnrollmentDto;
using University.Application.Models.DTOs.Staff;
using University.Domain.Entities.BaseEntities;
using University.Domain.Entities.JunctionEntities;
using University.Tests.Common;

namespace University.Tests.CreditWrokEnrollmentTests
{
    public class CreateCreditWorkEnrollmentCommandHandlerTests
    {
        [Fact]
        public async Task Handle_ShouldReturnBadRequest_WhenValidationFails()
        {
            var uow = new Mock<IUnitOfWork>();
            var creditWorkRepo = new Mock<ICreditWorkRepository>();
            var studentRepo = new Mock<IStudentRepository>();
            var enrollmentRepo = new Mock<ICreditWorkEnrollmentRepository>();

            var creditWorkId = Guid.NewGuid();
            var studentId = Guid.NewGuid();

            uow.SetupGet(x => x.CreditWorkRepository).Returns(creditWorkRepo.Object);
            uow.SetupGet(x => x.StudentRepository).Returns(studentRepo.Object);
            uow.SetupGet(x => x.CreditWorkEnrollmentRepository).Returns(enrollmentRepo.Object);

            creditWorkRepo.Setup(x => x.ExistsAsync(creditWorkId, It.IsAny<CancellationToken>())).ReturnsAsync(false);
            studentRepo.Setup(x => x.ExistsAsync(studentId, It.IsAny<CancellationToken>())).ReturnsAsync(true);

            var request = new CreateCreditWorkEnrollmentCommand
            {
                CreditWorkEnrollmentDto = new CreateCreditWorkEnrollmentDto
                {
                    CreditWorkId = creditWorkId,
                    StudentId = studentId
                }
            };

            var handler = new CreateCreditWorkEnrollmentCommandHandler(
                uow.Object,
                new Mock<ICurrentUserService>().Object,
                new Mock<IUserService>().Object);

            var result = await handler.Handle(request, CancellationToken.None);

            result.IsSuccessful.ShouldBeFalse();
            result.Status.ShouldBe(HttpStatusCode.BadRequest);

            uow.Verify(x => x.BeginTransactionAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Handle_ShouldCreateCreditWorkEnrollment()
        {
            var uow = new Mock<IUnitOfWork>();
            var creditWorkRepo = new Mock<ICreditWorkRepository>();
            var studentRepo = new Mock<IStudentRepository>();
            var enrollmentRepo = new Mock<ICreditWorkEnrollmentRepository>();
            var currentUser = new Mock<ICurrentUserService>();
            var userService = new Mock<IUserService>();

            var creditWorkId = Guid.NewGuid();
            var studentId = Guid.NewGuid();
            var staffId = Guid.NewGuid();

            var creditWork = EntityTestFactory.CreateCreditWork(creditWorkId);

            var student = EntityTestFactory.CreateStudent(studentId);

            var enrollment = EntityTestFactory.CreateCreditWorkEnrollment(student: student, creditWork: creditWork);

            uow.SetupGet(x => x.CreditWorkRepository).Returns(creditWorkRepo.Object);
            uow.SetupGet(x => x.StudentRepository).Returns(studentRepo.Object);
            uow.SetupGet(x => x.CreditWorkEnrollmentRepository).Returns(enrollmentRepo.Object);

            creditWorkRepo.Setup(x => x.ExistsAsync(creditWorkId, It.IsAny<CancellationToken>())).ReturnsAsync(true);
            studentRepo.Setup(x => x.ExistsAsync(studentId, It.IsAny<CancellationToken>())).ReturnsAsync(true);
            enrollmentRepo.Setup(x => x.ExistsAsync(studentId, creditWorkId, It.IsAny<CancellationToken>())).ReturnsAsync(false);

            creditWorkRepo.Setup(x => x.GetByIdAsync(creditWorkId, It.IsAny<CancellationToken>())).ReturnsAsync(creditWork);
            studentRepo.Setup(x => x.GetByIdAsync(studentId, It.IsAny<CancellationToken>())).ReturnsAsync(student);

            currentUser.SetupGet(x => x.UserId).Returns(staffId);
            userService.Setup(x => x.GetStaffByIdAsync(staffId))
                .ReturnsAsync(new StaffDto());

            enrollmentRepo.Setup(x => x.CreateCreditWorkEnrollment(It.IsAny<CreditWorkEnrollment>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(enrollment);

            var request = new CreateCreditWorkEnrollmentCommand
            {
                CreditWorkEnrollmentDto = new CreateCreditWorkEnrollmentDto
                {
                    CreditWorkId = creditWorkId,
                    StudentId = studentId
                }
            };

            var handler = new CreateCreditWorkEnrollmentCommandHandler(
                uow.Object,
                currentUser.Object,
                userService.Object);

            var result = await handler.Handle(request, CancellationToken.None);

            result.IsSuccessful.ShouldBeTrue();
            result.Status.ShouldBe(HttpStatusCode.Created);
            result.RecordId.ShouldBe(enrollment.Id);

            enrollmentRepo.Verify(
                x => x.CreateCreditWorkEnrollment(It.Is<CreditWorkEnrollment>(e =>
                    e.CreditWorkId == creditWorkId &&
                    e.StudentId == studentId &&
                    e.EnrolledById == staffId), CancellationToken.None),
                Times.Once);

            uow.Verify(x => x.BeginTransactionAsync(CancellationToken.None), Times.Once);
            uow.Verify(x => x.SaveChangesAsync(CancellationToken.None), Times.Once);
        }
    }
}
