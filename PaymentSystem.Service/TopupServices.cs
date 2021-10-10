using PaymentSystem.Repo.Dto;
using PaymentSystem.Repo.Interfaces;
using PaymentSystem.Service.Interfaces;

namespace PaymentSystem.Service
{
    public class TopupServices : ITopupService
    {
        ITopupRepo _repo;
        public TopupServices(ITopupRepo repo)
        {
            _repo = repo;
        }

        public decimal TopupBalance(TopupDto input)
        {
            return _repo.TopupBalance(input);
        }
    }
}
