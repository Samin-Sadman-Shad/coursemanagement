using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace University.Application.Exceptions
{
    public class ValidationException:ApplicationException
    {
        public List<string> Errors = new List<string>();
        public ValidationException(ValidationResult validationResult)
        {
            foreach(ValidationFailure error in validationResult.Errors)
            {
                Errors.Add(error.ErrorMessage);
            }
        }
    }
}
