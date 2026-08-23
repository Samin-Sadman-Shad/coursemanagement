using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using University.Identity.Models;

namespace University.Identity
{
    public class UniversityIdentityDbContext:IdentityDbContext<ApplicationUser>
    {
        public UniversityIdentityDbContext():base()
        {
            
        }
    }
}
