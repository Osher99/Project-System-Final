using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServerSideJWT.Models
{
    public class RegisterOutput
    {
        public IdentityResult Result { get; set; }

        public AppUser User { get; set; }
    }
}
