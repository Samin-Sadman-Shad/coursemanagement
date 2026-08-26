using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using University.Application.Contracts.Persistance;
using University.Application.Features.Student.Handlers.Commands;
using University.Application.Features.Student.Requests.Commands;
using Entities = University.Domain.Entities.BaseEntities;

namespace University.Tests.Student.Handlers
{
    public class DeleteStudentCommandHandlerTests
    {
        //[Fact]
        //public async Task Handle_ShouldDeleteStudent_WhenStudentExists()
        //{
        //    // Arrange
        //    var studentId = Guid.NewGuid();

        //    var student = new Entities.Student
        //    {
        //        Id = studentId
        //    };

        //    var studentRepositoryMock = new Mock<IStudentRepository>();
        //    var unitOfWorkMock = new Mock<IUnitOfWork>();

        //    studentRepositoryMock
        //        .Setup(x => x.GetByIdAsync(studentId))
        //        .ReturnsAsync(student);

        //    studentRepositoryMock
        //        .Setup(x => x.DeleteAsync(studentId))
        //        .Returns(Task.CompletedTask);

        //    unitOfWorkMock
        //        .SetupGet(x => x.StudentRepository)
        //        .Returns(studentRepositoryMock.Object);

        //    unitOfWorkMock
        //        .Setup(x => x.SaveChangesAsync())
        //        .Returns(Task.CompletedTask);

        //    var handler = new DeleteStudentCommandHandler(
        //        unitOfWorkMock.Object);

        //    var request = new DeleteStudentCommand
        //    {
        //        StudentId = studentId
        //    };

        //    // Act
        //    var result = await handler.Handle(
        //        request,
        //        CancellationToken.None);

        //    // Assert
        //    result.Should().NotBeNull();
        //    result.IsSuccessful.Should().BeTrue();
        //    result.Status.Should().Be(HttpStatusCode.NoContent);
        //    result.RecordId.Should().Be(studentId);

        //    studentRepositoryMock.Verify(
        //        x => x.GetByIdAsync(studentId),
        //        Times.Once);

        //    studentRepositoryMock.Verify(
        //        x => x.DeleteAsync(studentId),
        //        Times.Once);

        //    unitOfWorkMock.Verify(
        //        x => x.SaveChangesAsync(),
        //        Times.Once);
        //}
    }
}
