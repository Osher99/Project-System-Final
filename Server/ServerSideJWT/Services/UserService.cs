using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ServerSideJWT.Infra;
using ServerSideJWT.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Mail;
using System.Security.Claims;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace ServerSideJWT.Services
{
    public class UserService : IUserService
    {
        #region Fields

        public readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ApplicationSettings _appSettings;
        public readonly IEAEmailSender _emailSender;
        #endregion

        public UserService
        (
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            IOptions<ApplicationSettings> appSettings,
            IEAEmailSender emailSender
            )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
            _appSettings = appSettings.Value;
        }
        #region Methonds

        public async Task<AppUser> FindByEmail(string email)
        {
            try
            {
                return await _userManager.FindByEmailAsync(email);
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public async Task<AppUser> FindByID(string modelId)
        {
            return await _userManager.FindByIdAsync(modelId);
        }
        public async Task<IdentityResult> ConfirmEmail(AppUser user, string code)
        {
            return await _userManager.ConfirmEmailAsync(user, code);
        }

        public async Task<IdentityResult> ChangePassword(AppUser user, string code, string newPassword)
        {
            return await _userManager.ResetPasswordAsync(user, code, newPassword);
        }

        public async Task<RegisterOutput> Register(AppUserModel model)
        {
            var foundUser = await _userManager.FindByEmailAsync(model.Email);
            if (foundUser != null)
                throw new Exception("Email is already registered!");
 
            if (model.SpecialCode == _appSettings.Special_Code)
                model.Role = "admin";
            else
                model.Role = "customer";

            var appUser = new AppUser()
            {
                UserName = model.UserName,
                Email = model.Email,
                FullName = model.FullName,
            };

            try
            {
                var result = await _userManager.CreateAsync(appUser, model.Password);

                if (result.Succeeded)
                    await _userManager.AddToRoleAsync(appUser, model.Role);

                return new RegisterOutput
                {
                    Result = result,
                    User = appUser
                };
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<object> Login(LoginModel model)
        {
            var user = await _userManager.FindByNameAsync(model.UserName);

            if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
            {
                // Get the role assigned to the user
                var role = await _userManager.GetRolesAsync(user);
                IdentityOptions _options = new IdentityOptions();

                var tokenDecsriptor = new SecurityTokenDescriptor
                {
                    Subject = new System.Security.Claims.ClaimsIdentity(new Claim[]
                    {
                        new Claim("UserID", user.Id.ToString()),
                        new Claim(_options.ClaimsIdentity.RoleClaimType, role.FirstOrDefault())
                    }),
                    Expires = DateTime.UtcNow.AddDays(1),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey
                    (
                        Encoding.UTF8.GetBytes(_appSettings.Jwt_Secret)),
                        SecurityAlgorithms.HmacSha256Signature
                    )
                };

                var tokenHandler = new JwtSecurityTokenHandler();
                var securityToken = tokenHandler.CreateToken(tokenDecsriptor);
                var token = tokenHandler.WriteToken(securityToken);
                return token;
            }
            return null;
        }
        public async Task<object> GetUserProfile(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            return new
            {
                user.FullName,
                user.Email,
                user.UserName
            };
        }
        public async Task<bool> GetVerfied(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            return user.EmailConfirmed;     
        }
        public async Task<bool> GetRole(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            var role = await _userManager.GetRolesAsync(user);

            if (role.Any(u => u.Contains("admin")))
                return true;
            return false;
        }

        public async Task<string> GenerateEmailConfirmationTokenAsync(AppUser user)
        {
            return await _userManager.GenerateEmailConfirmationTokenAsync(user);
        }

        public string GetConfirmationToken(AppUser user)
        {
            return _userManager.GenerateEmailConfirmationTokenAsync(user).Result;
        }

        public bool SendEmail(AppUser user, string link)
        {
           return _emailSender.SendEmail(user, link);
        }
        public bool ResetPW(AppUser user, string link)
        {
            return _emailSender.ResetPassword(user, link);
        }

        public bool GenerateNewPassword(AppUser user, string newPass)
        {
            return _emailSender.GeneratePassword(user, newPass);
        }
        public async Task<string> GetRecoveryToken(AppUser user)
        {
            return await _userManager.GeneratePasswordResetTokenAsync(user);
        }

        public async Task<bool> ChangePasswordAsync(AppUser user, string currentPassword, string newPasword)
        {
            try
            {
                await _userManager.ChangePasswordAsync(user, currentPassword, newPasword);
                return true;
            }

            catch (Exception)
            {
                return false;
            }
        }

        #endregion

    }
}
