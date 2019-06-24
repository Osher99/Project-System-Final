using ServerSideJWT.Infra;
using ServerSideJWT.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServerSideJWT.Services
{
    public class ProjectService : IProjectService
    {
        private readonly IRepository _repositroy;
        public ProjectService(IRepository repositroy)
        {
            _repositroy = repositroy;
        }

        public async Task<bool> AddProject(ProjectModel model)
        {
            try
            {
                var newProject = new Project
                {
                    ProjectName = model.ProjectName,
                    Description = model.Description,
                    IsDone = false,
                    UserId = model.UserId,
                    ImageURL = model.ImageURL
                };

                return await _repositroy.AddProjectAsync(newProject);
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<bool> ChangeDone(int projectId)
        {
            try
            {
                return await _repositroy.ChangeDoneAsync(projectId);
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<bool> RemoveProject(int projectId)
        {
            try
            {              
                return await _repositroy.RemoveProjectAsync(projectId);
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<IEnumerable<Project>> GetAllProjects(string userId)
        {
            try
            {
                return await _repositroy.GetAllProjects(userId);
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
