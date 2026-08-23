using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using University.Application.Contracts.Persistance;
using University.Application.Utils;

namespace University.Application.Models.DTOs.StudentDTOs.Validators
{
    public class CreateStudentDtoValidator:AbstractValidator<CreateStudentDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        public CreateStudentDtoValidator(IUnitOfWork uow)
        {
            _unitOfWork = uow;
            Include(new IStudentDtoValidator(_unitOfWork));
        }
    }
}
