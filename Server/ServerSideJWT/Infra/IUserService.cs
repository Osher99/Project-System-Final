using Microsoft.AspNetCore.Identity;
using ServerSideJWT.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServerSideJWT.Infra
{
    public interface IUserService
    {
        Task<RegisterOutput> Register(AppUserModel model);
        Task<object> Login(LoginModel model);
        Task<object> GetUserProfile(string userId);
        Task<AppUser> FindByID(string modelId);
        Task<IdentityResult> ConfirmEmail(AppUser user, string code);
        Task<bool> GetVerfied(string userId);
        Task<AppUser> FindByEmail(string email);
        Task<string> GenerateEmailConfirmationTokenAsync(AppUser user);
        Task<bool> GetRole(string userId);
        Task<string> GetRecoveryToken(AppUser user);
        Task<IdentityResult> ChangePassword(AppUser user, string code, string newPassword);

        string GetConfirmationToken(AppUser user);
        bool SendEmail(AppUser user, string link);
        bool ResetPW(AppUser user, string link);
        bool GenerateNewPassword(AppUser user, string link);
        Task<bool> ChangePasswordAsync(AppUser user, string currentPassword, string newPasword);
    }
}
