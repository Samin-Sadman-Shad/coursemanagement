using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using University.Application.Contracts.Persistance;

namespace University.Application.Models.DTOs.CourseDTOs.Validators
{
    public class UpdateCourseTitleDtoValidator:AbstractValidator<UpdateCourseTitleDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        public UpdateCourseTitleDtoValidator(IUnitOfWork uow, Guid courseId)
        {
            _unitOfWork = uow;
            Include(new ICourseDtoValidator(_unitOfWork, courseId));
        }
    }
}
