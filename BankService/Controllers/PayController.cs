using Microsoft.AspNetCore.Mvc;
using PaymentSystem.Repo.Dto;
using PaymentSystem.Service.Interfaces;
using System;

namespace PaymentSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PayController : ControllerBase
    {
        IPayService _service;
        public PayController(IPayService service)
        {
            _service = service;
        }

        [HttpGet("{id}")]
        public BalanceDto GetBalance(Guid id)
        {
           return _service.GetBalance(id);
            
        }

        [HttpPost]
        [Route("Transfer")]
        public BalanceDto FundTransfer([FromBody] TransferDto input)
        {
            return _service.FundTransfer(input);
        }

    }
}
