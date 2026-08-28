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
using University.Application.Features.CourseCreditWorkRegistration.Handlers.Commands;
using University.Application.Features.CourseCreditWorkRegistration.Requests.Commands;
using University.Application.Models.DTOs.CourseCreditWorkRegistrationDTOs;
using University.Application.Models.DTOs.Staff;
using University.Domain.Entities.BaseEntities;
using University.Domain.Entities.JunctionEntities;
using University.Tests.Common;

namespace University.Tests.CourseToCreditWorkTests
{
    public class RegisterCourseToCreditWorkCommandHandlerTests
    {
        [Fact]
        public async Task Handle_ShouldRegisterCourseToCreditWork()
        {
            var uow = new Mock<IUnitOfWork>();
            var courseRepo = new Mock<ICourseRepository>();
            var creditWorkRepo = new Mock<ICreditWorkRepository>();
            var registrationRepo = new Mock<ICourseCreditWorkRegistrationRepository>();

            var courseId = Guid.NewGuid();
            var creditWorkId = Guid.NewGuid();
            var registration = EntityTestFactory.CreateCourseCreditWork();

            var staffId = Guid.NewGuid();

            uow.SetupGet(x => x.CourseRepository).Returns(courseRepo.Object);
            uow.SetupGet(x => x.CreditWorkRepository).Returns(creditWorkRepo.Object);
            uow.SetupGet(x => x.CourseCreditWorkRegistrationRepository).Returns(registrationRepo.Object);

            courseRepo.Setup(x => x.ExistsAsync(courseId)).ReturnsAsync(true);
            creditWorkRepo.Setup(x => x.ExistsAsync(creditWorkId)).ReturnsAsync(true);
            registrationRepo.Setup(x => x.ExistsAsync(courseId, creditWorkId)).ReturnsAsync(false);
            registrationRepo.Setup(x => x.RegisterCourseToCreditWork(courseId, creditWorkId, staffId))
                .ReturnsAsync(registration);

            var request = new RegisterCourseToCreditWorkCommand
            {
                courseCreditWorkDto = new CourseCreditWorkRegistrationDto
                {
                    CourseId = courseId,
                    CreditWorkId = creditWorkId
                }
            };

            var userService = new Mock<IUserService>();
            userService.Setup(x => x.GetStaffByIdAsync(staffId))
                .ReturnsAsync(new StaffDto());
            var currentUser = new Mock<ICurrentUserService>();
            currentUser.SetupGet(x => x.UserId).Returns(staffId);

            var handler = new RegisterCourseToCreditWorkCommandHandler(uow.Object, currentUser.Object, userService.Object);

            var result = await handler.Handle(request, CancellationToken.None);

            result.IsSuccessful.ShouldBeTrue();
            result.Status.ShouldBe(HttpStatusCode.Created);
            result.RecordId.ShouldBe(registration.Id);

            registrationRepo.Verify(x => x.RegisterCourseToCreditWork(courseId, creditWorkId, staffId), Times.Once);
            uow.Verify(x => x.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldReturnBadRequest_WhenRegistrationAlreadyExists()
        {
            var uow = new Mock<IUnitOfWork>();
            var courseRepo = new Mock<ICourseRepository>();
            var creditWorkRepo = new Mock<ICreditWorkRepository>();
            var registrationRepo = new Mock<ICourseCreditWorkRegistrationRepository>();

            var courseId = Guid.NewGuid();
            var creditWorkId = Guid.NewGuid();

            uow.SetupGet(x => x.CourseRepository).Returns(courseRepo.Object);
            uow.SetupGet(x => x.CreditWorkRepository).Returns(creditWorkRepo.Object);
            uow.SetupGet(x => x.CourseCreditWorkRegistrationRepository).Returns(registrationRepo.Object);

            courseRepo.Setup(x => x.ExistsAsync(courseId)).ReturnsAsync(true);
            creditWorkRepo.Setup(x => x.ExistsAsync(creditWorkId)).ReturnsAsync(true);
            registrationRepo.Setup(x => x.ExistsAsync(courseId, creditWorkId)).ReturnsAsync(true);

            var request = new RegisterCourseToCreditWorkCommand
            {
                courseCreditWorkDto = new CourseCreditWorkRegistrationDto
                {
                    CourseId = courseId,
                    CreditWorkId = creditWorkId
                }
            };

            var staffId = Guid.NewGuid();
            var userService = new Mock<IUserService>();
            userService.Setup(x => x.GetStaffByIdAsync(staffId))
                .ReturnsAsync(new StaffDto());
            var currentUser = new Mock<ICurrentUserService>();
            currentUser.SetupGet(x => x.UserId).Returns(staffId);

            var handler = new RegisterCourseToCreditWorkCommandHandler(uow.Object, currentUser.Object, userService.Object);

            var result = await handler.Handle(request, CancellationToken.None);

            result.IsSuccessful.ShouldBeFalse();
            result.Status.ShouldBe(HttpStatusCode.BadRequest);

            registrationRepo.Verify(x => x.RegisterCourseToCreditWork(It.IsAny<Guid>(), It.IsAny<Guid>(), staffId), Times.Never);
            uow.Verify(x => x.SaveChangesAsync(), Times.Never);
        }
    }
}
