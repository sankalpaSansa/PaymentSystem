using PaymentSystem.Repo.Dto;
using System;

namespace PaymentSystem.Repo.Interfaces
{
   public interface IPayRepo
    {
        BalanceDto GetBalance(Guid id);
        BalanceDto FundTransfer(TransferDto input);
        object GetAllWallets();
    }
}
