using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using University.Application.Utils;

namespace University.Application.Models.Identity
{
    public class RegistrationRequestValidator:AbstractValidator<RegistrationRequest>
    {
        public RegistrationRequestValidator()
        {
            RuleFor(req => req.FirstName).
               MaximumLength(20).
               WithMessage(CONST_STRING.PROPERTY_ERROR_MAX_LENGTH);

            RuleFor(req => req.LastName).
                MaximumLength(20).
                WithMessage(CONST_STRING.PROPERTY_ERROR_MAX_LENGTH);

            RuleFor(req => req.Email)
                .EmailAddress()
                .WithMessage(CONST_STRING.PROPERTY_ERROR_VALID_EMAIL);

            RuleFor(req => req.Password)
                .MinimumLength(5)
                .WithMessage(CONST_STRING.PROPERTY_ERROR_MIN_LENGTH);

        }
    }
  
}
