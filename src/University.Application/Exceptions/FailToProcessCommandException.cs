using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace University.Application.Exceptions
{
    public class FailToProcessCommandException:ApplicationException
    {
        public FailToProcessCommandException(Exception? inner=null, string? message = null):base(message, inner)
        {
            
        }

    }
}
