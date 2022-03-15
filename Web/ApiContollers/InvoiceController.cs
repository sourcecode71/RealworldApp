using Application.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PMG.Data.Repository.PayInvoice;
using PMG.Data.Repository.Projects;
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
            var invStatus = await _invRepository.SaveInvoice(invDTO);
            return Ok(invStatus);
        }

        [HttpGet("load-all-invoice")]
        public async Task<ActionResult> LoadInvoice()
        {
            var invStatus = await _invRepository.GetAllInvoice();
            return Ok(invStatus);
        }

        [HttpGet("work-order/load-invoices")]
        public async Task<ActionResult> LoadInvoiceByWrk(string wrkId)
        {
            var invStatus = await _invRepository.GetAllInvoice();
            return Ok(invStatus);
        }


    }
}
