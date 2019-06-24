using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServerSideJWT.Models
{
    public class ChangePasswordModel
    {
        public string Password { get; set; }
        public string CurrentPassword { get; set; }
    }
}
