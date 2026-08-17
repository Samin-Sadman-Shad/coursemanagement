using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using University.Domain.Entities.BaseEntities;

namespace University.Application.Models.DTOs.CreditWorkDTOs
{
    public static class CreditWorkDtoMapper
    {
        public static GetCreditWorkDto MapToGetCreditWorkDto(this CreditWork creditWork)
        {
            return new GetCreditWorkDto
            {
                Id = creditWork.Id,
                Heading = creditWork.Heading,
                Code = creditWork.Code,
                Description = creditWork.Description,
            };
        }

        public static CreditWork MaptoCreditWork(this CreateCreditWorkDto createCreditWorkDto)
        {
            return new CreditWork
            {
                Heading = createCreditWorkDto.Heading,
                Code = createCreditWorkDto.Code,
                Description = createCreditWorkDto.Description,
                CreatedBy = createCreditWorkDto.CreatedBy,
                CreatedAt = createCreditWorkDto.CreatedAt
            };
        }
    }
}
