using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using University.Application.Contracts.Persistance;
using University.Application.Features.Student.Requests.Commands;
using University.Application.Models.DTOs.StudentDTOs;
using University.Application.Models.DTOs.StudentDTOs.Validators;

namespace University.Application.Features.Student.Handlers.Commands
{
    public class UpdateStudentCommandHandler : IRequestHandler<UpdateStudentCommand, Unit>
    {
        private readonly IUnitOfWork _unitOfWork;
        public UpdateStudentCommandHandler(IUnitOfWork uow)
        {
            _unitOfWork = uow;
        }
        public async Task<Unit> Handle(UpdateStudentCommand request, CancellationToken cancellationToken)
        {
            var validator = new UpdateStudentDtoValidator();
            var validationResult = await validator.ValidateAsync(request.UpdateStudentDto, cancellationToken);
            if (!validationResult.IsValid)
            {
                throw new ArgumentException();
            }
            var studentRepository = _unitOfWork.StudentRepository;
            var entity = await studentRepository.GetByIdAsync(request.StudentId);
            if(entity is null)
            {
                throw new ArgumentException();
            }
            request.UpdateStudentDto.UpdateStudent(entity);
            await _unitOfWork.SaveChangesAsync();
            return Unit.Value;
        }
    }
}
