using Microsoft.AspNetCore.Mvc;
using PaymentSystem.Repo.Dto;
using PaymentSystem.Service.Interfaces;

namespace PaymentSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TopupController : ControllerBase
    {

        ITopupService _service;
        public TopupController(ITopupService service)
        {
            _service = service;
        }

        [HttpPost]
        public decimal Topup([FromBody] TopupDto input)
        {
           return _service.TopupBalance(input);
        }

    }
}
