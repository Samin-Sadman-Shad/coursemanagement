using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace University.Application.Features.Student.Requests.Commands
{
    public interface IStudentUpdateCommand
    {
        public Guid StudentId { get; set; }
    }
}
