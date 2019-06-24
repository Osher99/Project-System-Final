using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Mail;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using EASendMail;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ServerSideJWT.Infra;
using ServerSideJWT.Models;
using ServerSideJWT.Services;

namespace ServerSideJWT.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppUserController : ControllerBase
    {
        #region Fields
        private const string EMAIL_VERIFIED_URI = "http://localhost:4200/user/login";
        private readonly IUserService _userService;
        #endregion

        #region Ctor

        public AppUserController(IUserService userService)
        {
            _userService = userService;
        }
        #endregion

        #region Requests

        #region Register
        //Post Request
        //POST : /api/AppUser/Register
        [HttpPost]
        [Route("Register")]
        public async Task<object> PostAppUser(AppUserModel model)
        {
            try
            {
                var output = await _userService.Register(model);

                if (output.Result.Succeeded)
                {
                    string confirmationToken = _userService.GetConfirmationToken(output.User);

                    string confirmationTokenLink = Url.Action("ConfirmEmail", "AppUser", new
                    {
                        userId = output.User.Id,
                        code = confirmationToken
                    }, protocol: HttpContext.Request.Scheme);

                    bool finalRes = _userService.SendEmail(output.User, confirmationTokenLink);

                    if (finalRes)
                        return Ok(output.Result);

                    return BadRequest(new { message = "Register failed! Email verfication has been destoryed" });
                }
                else
                    return BadRequest(new { message = "Register failed! Please try again later" });
            }

            catch (Exception e)
            {
                return BadRequest(new { e.Message });
            }
        }
        #endregion
        //Post Request
        //POST : /api/AppUser/Login
        #region Login
        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login(LoginModel model)
        {
            var token = await _userService.Login(model);

            if (token != null)
                return Ok(new { token });
            else
                return BadRequest(new { message = "Username or password is incorrect! Try again please" });
        }
        #endregion
        //Post Request
        //POST : /api/AppUser/ChangePassword
        #region Login
        [HttpPost]
        [Route("ChangePassword")]
        public async Task<IActionResult> ChangePassword(ChangePasswordModel model)
        {
            string userId = User.Claims.First(c => c.Type == "UserID").Value;
            var user = await _userService.FindByID(userId);
       
            try
            {
                var result = await _userService.ChangePasswordAsync(user, model.CurrentPassword, model.Password);
                return Ok(result);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
        #endregion


        #region EmailConfirmation
        [HttpGet("[action]")]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(string userId, string code)
        {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(code))
                return BadRequest(new { message = "Code or ID in empty" });

            var user = await _userService.FindByID(userId);

            if (user == null)
                return BadRequest(new { message = "User not found!" });

            if (user.EmailConfirmed)
                return Content($"<html><body><h1>User already confirmed is email! </ h1 >< hr >< a href = { EMAIL_VERIFIED_URI }> Please Login now! </ a >/body></html>");

            var result = await _userService.ConfirmEmail(user, code);

            if (result.Succeeded)
                return Content($"<html><body><h1>Your're email has been verified!!!</h1><hr><a href= { EMAIL_VERIFIED_URI }> Please Login now! </a></body></html>", "text/html");

            else
                return BadRequest(new { message = "Verfication failed!" });

        }
        #endregion

        //Post Request
        #region Recover Password
        [HttpPost]
        [Route("RecoverPassword")]
        public async Task<IActionResult> RecoverPassword(RecoverPasswordModel model)
        {

            var user = await _userService.FindByEmail(model.Email);
            string code = await _userService.GetRecoveryToken(user);

            string recoveryLink = Url.Action("RecoveryLink", "AppUser", new
            {
                UserId = user.Id,
                Code = code
            }, protocol: HttpContext.Request.Scheme);

            bool finalRes = _userService.ResetPW(user, recoveryLink);

            if (finalRes)
                return Ok(finalRes);
            else
                return BadRequest(new { message = "No such user with this email" });
        }
        #endregion
        
        #region PasswordRecoveryAction

        [HttpGet("[action]")]
        [AllowAnonymous]
        public async Task<IActionResult> RecoveryLink(string userId, string code)
        {
            Random random = new Random();
            object[] chars = new object[]
            {1, 2, 3, 4, 5, 6, 7, 8, 9, 0,
                'a', 'b', 'c', 'd' ,'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l',
                'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v' ,'w', 'x', 'y', 'z' };

            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(code))
                return BadRequest(new { message = "Code or ID in empty" });

            var user = await _userService.FindByID(userId);

            if (user == null)
                return BadRequest(new { message = "User not found!" });

            StringBuilder newPass = new StringBuilder();

            for (int i = 0; i < 7; i++)
                newPass.Append(chars[random.Next(chars.Length)]);
            
                var result = await _userService.ChangePassword(user, code, newPass.ToString());
            var emailSent =  _userService.GenerateNewPassword(user, newPass.ToString());

            if (result.Succeeded)
            {
                if (emailSent)
                    return Content($"<html><body><h1>Please check your inbox for your new password!!!</h1><hr></body></html>", "text/html");

                return BadRequest("Email faild to send");
            }

            else
                return BadRequest(new { message = "Reset failed!" });

        }
        #endregion

        #endregion
    }
}