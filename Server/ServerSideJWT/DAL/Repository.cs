using Microsoft.EntityFrameworkCore;
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

        public async Task<IEnumerable<PaymentDetail>> GetPaymentDetails(string userId)
        {
            return await _context.PaymentDetails.Where(p=> p.UserId == userId).ToListAsync();
        }

        public async Task<bool> AddCard(PaymentDetail paymentDetail)
        {
            try
            {
                _context.PaymentDetails.Add(paymentDetail);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }

        public async Task<PaymentDetail> GetPaymentDetail(int id)
        {
            var paymentDetail = await _context.PaymentDetails.FindAsync(id);

            return paymentDetail;
        }

        public async Task<bool> EditCardDetail(int id, PaymentDetail paymentDetail)
        {

            if (id != paymentDetail.PMId)
                return false;

            _context.Entry(paymentDetail).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                if (!PaymentDetailExists(id))
                {
                    return false;
                }
                throw;
            }
            return true;
        }

        public  bool PaymentDetailExists(int id)
        {
            return _context.PaymentDetails.Any(e => e.PMId == id);
        }

        public async Task<PaymentDetail> DeletePaymentDetail(int id)
        {
            var paymentDetail = await _context.PaymentDetails.FindAsync(id);
            if (paymentDetail == null)
            {
                return null;
            }

            _context.PaymentDetails.Remove(paymentDetail);
            await _context.SaveChangesAsync();

            return paymentDetail;
        }
    }
}
