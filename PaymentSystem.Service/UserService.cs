using PaymentSystem.Repo.Dto;
using PaymentSystem.Repo.Interfaces;
using PaymentSystem.Service.Interfaces;


namespace PaymentSystem.Service
{
    public class UserService : IUserService
    {
        IUserRepo _repo;
        public UserService(IUserRepo repo)
        {
            _repo = repo;
        }

        public UserDto Authenticate(string UserName)
        {
            var user = _repo.GetUser(UserName);
            if (user != null)
                return user;
            return _repo.CreateUser(UserName);
        }
    }
}
