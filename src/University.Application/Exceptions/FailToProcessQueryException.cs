using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace University.Application.Exceptions
{
    public class FailToProcessQueryException:ApplicationException
    {
        public FailToProcessQueryException( Exception inner, string? message = null ) :base(message, inner)
        {
            
        }
    }
}
