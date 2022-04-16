using Application.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PMG.Data.Repository.PayInvoice;
using System.Threading.Tasks;

namespace Web.ApiContollers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InvoiceController : ControllerBase
    {
        private IInvoiceRepository _invRepository;

        public InvoiceController(IInvoiceRepository invRepository)
        {
            _invRepository = invRepository;
        }


        [HttpPost("save-invoice")]
        public async Task<ActionResult> SaveInvoice(InvoiceDTO invDTO)
        {
            string currentEmail = HttpContext.Session.GetString("current_user_email");
            if (!string.IsNullOrEmpty(currentEmail))
            {
                invDTO.SetUser = HttpContext.Session.GetString("current_user_id");
                var invStatus = await _invRepository.SaveInvoice(invDTO);
                string OpStatus = invStatus ? "Success" : "Fail";
                return Ok(OpStatus);
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpGet("load-all-invoice")]
        public async Task<ActionResult> LoadInvoice()
        {
            var invStatus = await _invRepository.GetAllInvoice();
            return Ok(invStatus);
        }

        [HttpGet("load-pending-invoice")]
        public async Task<ActionResult> LoadPendingInvoice()
        {
            try
            {
                var invStatus = await _invRepository.GetPendingInvoice();
                return Ok(invStatus);
            }
            catch (System.Exception ex)
            {

                throw ex;
            }
        }

        [HttpGet("work-order/load-invoices")]
        public async Task<ActionResult> LoadInvoiceByWrk(string wrkId)
        {
            var invStatus = await _invRepository.GetAllInvoice();
            return Ok(invStatus);
        }

        [HttpPost("pay-bill")]
        public async Task<ActionResult> SavePayment(PaymentDto dto)
        {
            string currentEmail = HttpContext.Session.GetString("current_user_email");
            if (!string.IsNullOrEmpty(currentEmail))
            {
                dto.SetUser = HttpContext.Session.GetString("current_user_id");
                var invStatus = await _invRepository.SavePayBill(dto);
                return Ok(invStatus);
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpGet("load-all-payment")]
        public async Task<ActionResult> GetAllPayment()
        {
            var pays = await _invRepository.GetAllPayment();
            return Ok(pays);
        }



    }
}
