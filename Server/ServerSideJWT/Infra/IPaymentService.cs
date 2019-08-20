using ServerSideJWT.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServerSideJWT.Infra
{
    public interface IPaymentService
    {
        Task<IEnumerable<PaymentDetail>> GetAllProjects(string userId);
        Task<bool> AddCard(PaymentDetail paymentDetail);
        Task<PaymentDetail> FindCard(int id);
        Task<bool> EditCard(int id, PaymentDetail paymentDetail);
        Task<PaymentDetail> DeleteCard(int id);

    }
}
