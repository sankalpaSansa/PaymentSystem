using PaymentSystem.Repo.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace PaymentSystem.Repo.Interfaces
{
    public interface ITopupRepo
    {
        decimal TopupBalance(TopupDto input);
    }
}
