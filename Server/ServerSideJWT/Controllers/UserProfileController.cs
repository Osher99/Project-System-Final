using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using ServerSideJWT.Infra;
using ServerSideJWT.Models;
using ServerSideJWT.Services;

namespace ServerSideJWT.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserProfileController : ControllerBase
    {
        #region Fields
        private readonly IUserService _userService;
        #endregion
        public UserProfileController(IUserService userService)
        {
            _userService = userService;
        }
        #region Requests
        [HttpGet]
        [Route("IsVerified")]
        public async Task<bool> IsVerified()
        {
            string userId = User.Claims.First(c => c.Type == "UserID").Value;
            return await _userService.GetVerfied(userId);
        }
        [HttpGet]
        [Route("IsAdmin")]
        public async Task<bool> IsAdmin()
        {
            string userId = User.Claims.First(c => c.Type == "UserID").Value;
            return await _userService.GetRole(userId);
        }
        //GET : /api/UserProfile
        [HttpGet]
        [Authorize]
        public async Task<object> GetUserProfile()
        {
            string userId = User.Claims.First(c => c.Type == "UserID").Value;
            return await _userService.GetUserProfile(userId);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        [Route("ForAdmin")]
        public string GetForAdmin()
        {
            return "web method for Admin";
        }

        [HttpGet]
        [Authorize(Roles = "customer")]
        [Route("ForCustomer")]
        public string GetForCustomer()
        {
            return "web method for Customer";
        }

        //[HttpGet]
        //[Authorize(Roles = "admin,customer")]
        //[Route("ForAdminOrCustomer")]
        //public string GetForAdminOrCustomer()
        //{
        //    return "web method for Admin or Customer";
        //}
        #endregion
    }
}