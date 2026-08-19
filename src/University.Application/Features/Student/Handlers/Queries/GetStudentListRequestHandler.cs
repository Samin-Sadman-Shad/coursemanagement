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
    public class GetStudentListRequestHandler : IRequestHandler<GetStudentListRequest, List<GetStudentDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        public GetStudentListRequestHandler(IUnitOfWork uow)
        {
            _unitOfWork = uow;
        }
        public async Task<List<GetStudentDto>> Handle(GetStudentListRequest request, CancellationToken cancellationToken)
        {
            var studentRepository = _unitOfWork.StudentRepository;
            var entities = await studentRepository.GetAllAsync();
            return entities.Select(entity => entity.MapToGetStudentDto()).ToList();
        }
    }
}
