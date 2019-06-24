using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServerSideJWT.Models
{
    public class ApplicationSettings
    {
        public string Jwt_Secret { get; set; }
        public string Client_URL { get; set; }
        public string Special_Code { get; set; }
        public string Email_Address { get; set; }
        public string Password_Secret { get; set; }

    }
}
