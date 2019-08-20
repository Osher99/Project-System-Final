using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ServerSideJWT.Data;
using ServerSideJWT.Infra;
using ServerSideJWT.Models;

namespace ServerSideJWT.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentDetailController : ControllerBase
    {
        private readonly AuthContext _context;
        private readonly IPaymentService _paymentService;

        public PaymentDetailController(AuthContext context, IPaymentService paymentService)
        {
            _context = context;
            _paymentService = paymentService;
        }

        // GET: api/PaymentDetail
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PaymentDetail>>> GetPaymentDetails()
        {
            string userId = User.Claims.First(c => c.Type == "UserID").Value;

            var cardList = await _paymentService.GetAllProjects(userId);

            if (cardList == null)
                return NotFound();

            return Ok(cardList);

        }

        // GET: api/PaymentDetail/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PaymentDetail>> GetPaymentDetail(int id)
        {
            var paymentDetail = await _paymentService.FindCard(id);

            if (paymentDetail == null)
                return NotFound();

            return paymentDetail;
        }

        // PUT: api/PaymentDetail/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPaymentDetail(int id, PaymentDetail paymentDetail)
        {
            var result = await _paymentService.EditCard(id, paymentDetail);

            if (result)
                return Ok();
            return NotFound();
        }

        // POST: api/PaymentDetail
        [HttpPost]
        public async Task<ActionResult<PaymentDetail>> PostPaymentDetail(PaymentDetail paymentDetail)
        {
            string userId = User.Claims.First(c => c.Type == "UserID").Value;

            paymentDetail.UserId = userId;

            bool result = await _paymentService.AddCard(paymentDetail);

            if (result)
                return CreatedAtAction("GetPaymentDetail", new { id = paymentDetail.PMId }, paymentDetail);
            else
                return NotFound();

        }

        // DELETE: api/PaymentDetail/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<PaymentDetail>> DeletePaymentDetail(int id)
        {
            var paymentDeleted = await _paymentService.DeleteCard(id);

            if (paymentDeleted == null)
                return NotFound();

            return paymentDeleted;
        }

    }
}
