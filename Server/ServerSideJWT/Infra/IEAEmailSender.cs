using ServerSideJWT.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServerSideJWT.Infra
{
    public interface IEAEmailSender
    {
        bool SendEmail(AppUser user, string confirmationTokenLink);
        bool ResetPassword(AppUser user, string confirmationTokenLink);
        bool GeneratePassword(AppUser user, string newPassword);
    }
}
