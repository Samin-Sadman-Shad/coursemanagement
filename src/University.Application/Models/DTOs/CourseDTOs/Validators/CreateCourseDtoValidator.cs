using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using University.Application.Contracts.Persistance;

namespace University.Application.Models.DTOs.CourseDTOs.Validators
{
    public class CreateCourseDtoValidator:AbstractValidator<CreateCourseDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        public CreateCourseDtoValidator(IUnitOfWork uow)
        {
            _unitOfWork = uow;
            Include(new ICourseDtoValidator(_unitOfWork));
        }
    }
}
