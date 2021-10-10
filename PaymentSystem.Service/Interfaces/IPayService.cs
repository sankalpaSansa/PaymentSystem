using PaymentSystem.Repo.Dto;
using System;

namespace PaymentSystem.Service.Interfaces
{
    public interface IPayService
    {
        BalanceDto GetBalance(Guid id);
        BalanceDto FundTransfer(TransferDto input);
        object GetAllWallets();
    }
}
