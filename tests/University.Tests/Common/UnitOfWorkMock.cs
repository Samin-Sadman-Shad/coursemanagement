using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using University.Application.Contracts.Persistance;

namespace University.Tests.Common
{
    public static class UnitOfWorkMock
    {
        public static Mock<IUnitOfWork> Create(
            Mock<IStudentRepository>? studentRepo = null,
            Mock<ICourseRepository>? courseRepo = null,
            Mock<ICourseEnrollmentRepository>? courseEnrollmentRepo = null,
            Mock<ICreditWorkRepository>? creditWorkRepo = null,
            Mock<ICreditWorkEnrollmentRepository>? creditWorkEnrollmentRepo = null,
            Mock<ICourseCreditWorkRegistrationRepository>? courseCreditWorkRegistrationRepo = null)
        {
            var unitOfWork = new Mock<IUnitOfWork>();

            unitOfWork.Setup(u => u.StudentRepository)
                .Returns((studentRepo ?? new Mock<IStudentRepository>()).Object);

            unitOfWork.Setup(u => u.CourseRepository)
                .Returns((courseRepo ?? new Mock<ICourseRepository>()).Object);

            unitOfWork.Setup(u => u.CourseEnrollmentRepository)
                .Returns((courseEnrollmentRepo ?? new Mock<ICourseEnrollmentRepository>()).Object);

            unitOfWork.Setup(u => u.CreditWorkRepository)
                .Returns((creditWorkRepo ?? new Mock<ICreditWorkRepository>()).Object);

            unitOfWork.Setup(u => u.CreditWorkEnrollmentRepository)
                .Returns((creditWorkEnrollmentRepo ?? new Mock<ICreditWorkEnrollmentRepository>()).Object);

            unitOfWork.Setup(u => u.CourseCreditWorkRegistrationRepository)
                .Returns((courseCreditWorkRegistrationRepo ?? new Mock<ICourseCreditWorkRegistrationRepository>()).Object);

            return unitOfWork;
        }
    }
}
