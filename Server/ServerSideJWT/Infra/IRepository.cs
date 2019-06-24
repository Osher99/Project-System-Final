using ServerSideJWT.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServerSideJWT.Infra
{
    public interface IRepository
    {
        Task<bool> AddProjectAsync(Project project);
        Task<bool> RemoveProjectAsync(int projectId);
        Project FindProjectById(int projectId);
        Task<IEnumerable<Project>> GetAllProjects(string userId);
        Task<bool> ChangeDoneAsync(int projectId);

    }
}
