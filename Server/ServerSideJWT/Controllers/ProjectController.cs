using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServerSideJWT.Infra;
using ServerSideJWT.Models;
using ServerSideJWT.Services;

namespace ServerSideJWT.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectController : ControllerBase
    {
        private readonly IProjectService _porjectService;

        public ProjectController(IProjectService porjectService)
        {
            _porjectService = porjectService;
        }

        [HttpPost]
        [Route("AddProject")]
        public async Task<object> PostProject(ProjectModel model)
        {
            try
            {
                string userId = User.Claims.First(c => c.Type == "UserID").Value;
                model.UserId = userId;

                var Result = await _porjectService.AddProject(model);

               if (Result)
                     return Ok(Result);

                return BadRequest(new { message = "Register failed! Email verfication has been destoryed" });
            
            }

            catch (Exception e)
            {
                return BadRequest(new { e.Message });
            }
        }

        [HttpPost]
        [Route("RemoveProject")]
        public async Task<object> RemoveProject(Project model)
        {
            try
            {
                var Result = await _porjectService.RemoveProject(model.Id);

                if (Result)
                    return Ok(Result);

                else
                    return BadRequest(new { message = "Project cannot be added right now" });
            }

            catch (Exception ex)
            {
                return BadRequest(new { ex.Message });
            }
        }

        [HttpPost]
        [Route("ChangeDone")]
        public async Task<object> ChangeDone(Project model)
        {
            try
            {
                var Result = await _porjectService.ChangeDone(model.Id);

                if (Result)
                    return Ok(Result);

                else
                    return BadRequest(new { message = "Project cannot be added right now" });
            }

            catch (Exception ex)
            {
                return BadRequest(new { ex.Message });
            }
        }
        [HttpGet]
        [Route("GetAllProjects")]
        public async Task<IEnumerable<Project>> GetAllProjects()
        {
            try
            {
                string userId = User.Claims.First(c => c.Type == "UserID").Value;

                try
                {
                    var projectList = await _porjectService.GetAllProjects(userId);
                    return projectList;
                }

                catch (Exception)
                {
                    return null;
                }
            }

            catch (Exception)
            {
                return null;
            }
        }

    }
}