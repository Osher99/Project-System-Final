using ServerSideJWT.Data;
using ServerSideJWT.Infra;
using ServerSideJWT.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServerSideJWT.DAL
{
    public class Repository: IRepository
    {
        private readonly AuthContext _context;
        public Repository(AuthContext context)
        {
            _context = context;
        }
        public async Task<bool> AddProjectAsync(Project project)
        {
            await _context.Projects.AddAsync(project);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RemoveProjectAsync(int projectId)
        {
            var projestToRemove = FindProjectById(projectId);

            if (projestToRemove != null)
            {
                _context.Projects.Remove(projestToRemove);
                await _context.SaveChangesAsync();
                return true;
            }

            else
                return false;
        }

        public Project FindProjectById(int projectId)
        {
            return _context.Projects.FirstOrDefault(p => p.Id == projectId);
        }
        public async Task<IEnumerable<Project>> GetAllProjects(string userId)
        {
            return _context.Projects.Where(p => p.UserId == userId).ToList();
        }
        public async Task<bool> ChangeDoneAsync(int projectId)
        {
            var projectToChange = FindProjectById(projectId);

            if (projectToChange != null)
            {
                _context.Projects.Where(p => p.Id == projectId).FirstOrDefault().IsDone =
                    !_context.Projects.Where(p => p.Id == projectId).FirstOrDefault().IsDone;
                await _context.SaveChangesAsync();
                return true;
            }

            else
                return false;
        }

    }
}
