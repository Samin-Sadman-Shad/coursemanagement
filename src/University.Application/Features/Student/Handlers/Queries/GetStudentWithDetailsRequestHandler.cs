using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using University.Application.Contracts.Persistance;
using University.Application.Features.Student.Requests.Queries;
using University.Application.Models.DTOs.StudentDTOs;

namespace University.Application.Features.Student.Handlers.Queries
{
    public class GetStudentWithDetailsRequestHandler : IRequestHandler<GetStudentWithDetailsRequest, GetStudentWithDetailsDto?>
    {
        private readonly IUnitOfWork _unitOfWork;
        public GetStudentWithDetailsRequestHandler(IUnitOfWork uow)
        {
            _unitOfWork = uow;
        }
        public async Task<GetStudentWithDetailsDto?> Handle(GetStudentWithDetailsRequest request, CancellationToken cancellationToken)
        {
            var studentRepository = _unitOfWork.StudentRepository;
            var student = await studentRepository.GetByIdDetailAsync(request.studentId);
            
            return (student is not null)? student.MapToGetStudentWithDetailsDto() : null;
            
        }
    }
}
