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
        Task<IEnumerable<PaymentDetail>> GetPaymentDetails(string userId);
        Task<bool> AddCard(PaymentDetail paymentDetail);
        Task<PaymentDetail> GetPaymentDetail(int id);
        Task<bool> EditCardDetail(int id, PaymentDetail paymentDetail);



    }
}
