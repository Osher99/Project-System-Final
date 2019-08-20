using ServerSideJWT.Infra;
using ServerSideJWT.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServerSideJWT.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IRepository _repositroy;

        public PaymentService(IRepository repository)
        {
            _repositroy = repository;
        }

        public async Task<bool> AddCard(PaymentDetail paymentDetail)
        {
            return await _repositroy.AddCard(paymentDetail);
        }

        public Task<PaymentDetail> DeleteCard(int id)
        {
            return _repositroy.DeletePaymentDetail(id);
        }

        public async Task<bool> EditCard(int id, PaymentDetail paymentDetail)
        {
            return await _repositroy.EditCardDetail(id, paymentDetail);
        }

        public async Task<PaymentDetail> FindCard(int id)
        {
            return await _repositroy.GetPaymentDetail(id);
        }

        public async Task<IEnumerable<PaymentDetail>> GetAllProjects(string userId)
        {
            return await _repositroy.GetPaymentDetails(userId);
        }
    }
}
