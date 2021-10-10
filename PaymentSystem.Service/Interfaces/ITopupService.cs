using PaymentSystem.Repo.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace PaymentSystem.Service.Interfaces
{
    public interface ITopupService
    {
        decimal TopupBalance(TopupDto input);
    }
}
