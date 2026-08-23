using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using University.Application.Contracts.Persistance;

namespace University.Application.Models.DTOs.StudentDTOs.Validators
{
    public class UpdateStudentDtoValidator:AbstractValidator<UpdateStudentDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        public UpdateStudentDtoValidator(IUnitOfWork uow)
        {
            _unitOfWork = uow;
            Include(new IStudentDtoValidator(_unitOfWork));
        }
    }
}
