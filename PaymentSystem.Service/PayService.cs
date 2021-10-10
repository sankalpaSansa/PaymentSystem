using PaymentSystem.Repo;
using PaymentSystem.Repo.Dto;
using PaymentSystem.Repo.Interfaces;
using PaymentSystem.Service.Interfaces;
using System;

namespace PaymentSystem.Service
{
    public class PayService : IPayService
    {
        IPayRepo _repo;
        public PayService(IPayRepo repo)
        {
            _repo = repo;
        }

        public object GetAllWallets()
        {
            return _repo.GetAllWallets();
        }

        public BalanceDto GetBalance(Guid id)
        {
            return _repo.GetBalance(id);
        }

        public BalanceDto FundTransfer(TransferDto input)
        {
            return _repo.FundTransfer(input);
        }

    }
}
