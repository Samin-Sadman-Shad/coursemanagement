using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using University.Application.Contracts.Persistance;
using University.Application.Features.Student.Requests.Commands;
using University.Application.Models.DTOs.StudentDTOs;
using University.Application.Models.DTOs.StudentDTOs.Validators;

namespace University.Application.Features.Student.Handlers.Commands
{
    public class CreateStudentCommandHandler : IRequestHandler<CreateStudentCommand, Guid>
    {
        private readonly IUnitOfWork _unitOfWork;
        public CreateStudentCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Guid> Handle(CreateStudentCommand request, CancellationToken cancellationToken)
        {
            var studentRepository = _unitOfWork.StudentRepository;
            var dto = request.createStudentDto;
            var validator = new CreateStudentDtoValidator();
            var validationResult = await validator.ValidateAsync(dto);
            if (!validationResult.IsValid)
            {
                throw new ArgumentException("Request is not valid");
            }
            var entity = dto.MapToStudent();
            await studentRepository.CreateAsync(entity);
            await _unitOfWork.SaveChangesAsync();
            return entity.UserId;

        }
    }
}
