using ServerSideJWT.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServerSideJWT.Infra
{
    public interface IProjectService
    {
        Task<bool> AddProject(ProjectModel model);
        Task<bool> RemoveProject(int projectId);
        Task<IEnumerable<Project>> GetAllProjects(string userId);
        Task<bool> ChangeDone(int projectId);

    }
}
