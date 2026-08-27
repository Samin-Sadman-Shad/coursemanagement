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
using University.Application.Features.CourseEnrollment.Handlers.Commands;
using University.Application.Features.CourseEnrollment.Requests.Commands;
using University.Application.Models.DTOs.CourseEnrollmentDTOs;
using University.Application.Models.DTOs.Staff;
using University.Domain.Entities.BaseEntities;
using University.Domain.Entities.JunctionEntities;
using University.Tests.Common;

namespace University.Tests.CourseEnrollmentTests
{
    public class CreateCourseEnrollmentCommandHandlerTests
    {
        [Fact]
        public async Task Handle_ShouldReturnNotFound_WhenStudentDoesNotExist()
        {
            var uow = new Mock<IUnitOfWork>();
            var studentRepo = new Mock<IStudentRepository>();
            var courseRepo = new Mock<ICourseRepository>();

            var studentId = Guid.NewGuid();
            var courseId = Guid.NewGuid();

            uow.SetupGet(x => x.StudentRepository).Returns(studentRepo.Object);
            uow.SetupGet(x => x.CourseRepository).Returns(courseRepo.Object);

            studentRepo.Setup(x => x.GetByIdAsync(studentId))
                .ReturnsAsync((Student?)null);

            courseRepo.Setup(x => x.GetByIdAsync(courseId))
                .ReturnsAsync(EntityTestFactory.CreateCourse(courseId));;

            var request = new CreateCourseEnrollmentCommand
            {
                CourseEnrollmentDto = new CreateCourseEnrollmentDto
                {
                    StudentId = studentId,
                    CourseId = courseId
                }
            };

            var handler = new CreateCourseEnrollmentCommandHandler(
                uow.Object,
                new Mock<ICurrentUserService>().Object,
                new Mock<IUserService>().Object);

            var result = await handler.Handle(request, CancellationToken.None);

            result.IsSuccessful.ShouldBeFalse();
            result.Status.ShouldBe(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Handle_ShouldCreateCourseEnrollment()
        {
            var uow = new Mock<IUnitOfWork>();
            var studentRepo = new Mock<IStudentRepository>();
            var courseRepo = new Mock<ICourseRepository>();
            var enrollmentRepo = new Mock<ICourseEnrollmentRepository>();
            var currentUser = new Mock<ICurrentUserService>();
            var userService = new Mock<IUserService>();

            var studentId = Guid.NewGuid();
            var courseId = Guid.NewGuid();
            var staffId = Guid.NewGuid();

            var student = EntityTestFactory.CreateStudent(studentId);

            var course = EntityTestFactory.CreateCourse("Programming", courseId);

            var enrollment = EntityTestFactory.CreateCourseEnrollment(Guid.NewGuid(), staffId, student, course);

            uow.SetupGet(x => x.StudentRepository).Returns(studentRepo.Object);
            uow.SetupGet(x => x.CourseRepository).Returns(courseRepo.Object);
            uow.SetupGet(x => x.CourseEnrollmentRepository).Returns(enrollmentRepo.Object);

            studentRepo.Setup(x => x.GetByIdAsync(studentId)).ReturnsAsync(student);
            courseRepo.Setup(x => x.GetByIdAsync(courseId)).ReturnsAsync(course);

            studentRepo
                .Setup(x => x.ExistsAsync(studentId))
                .ReturnsAsync(true);

            courseRepo
                .Setup(x => x.ExistsAsync(courseId))
                .ReturnsAsync(true);

            currentUser.SetupGet(x => x.UserId).Returns(staffId);
            userService.Setup(x => x.GetStaffByIdAsync(staffId))
                .ReturnsAsync(new StaffDto());

            enrollmentRepo.Setup(x => x.CreateCourseEnrollment(It.IsAny<CourseEnrollment>()))
                .ReturnsAsync(enrollment);

            var request = new CreateCourseEnrollmentCommand
            {
                CourseEnrollmentDto = new CreateCourseEnrollmentDto
                {
                    StudentId = studentId,
                    CourseId = courseId
                }
            };

            var handler = new CreateCourseEnrollmentCommandHandler(
                uow.Object,
                currentUser.Object,
                userService.Object);

            var result = await handler.Handle(request, CancellationToken.None);

            
            result.Status.ShouldBe(HttpStatusCode.Created);
            result.RecordId.ShouldBe(enrollment.Id);
            result.IsSuccessful.ShouldBeTrue();

            enrollmentRepo.Verify(
                x => x.CreateCourseEnrollment(It.Is<CourseEnrollment>(e =>
                    e.StudentId == studentId &&
                    e.CourseId == courseId &&
                    e.EnrolledById == staffId)),
                Times.Once);

            uow.Verify(x => x.SaveChangesAsync(), Times.Once);
        }
    }
}
