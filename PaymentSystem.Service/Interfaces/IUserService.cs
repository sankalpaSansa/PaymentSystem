using PaymentSystem.Repo.Dto;

namespace PaymentSystem.Service.Interfaces
{
    public interface IUserService
    {
        UserDto Authenticate(string UserName);
    }
}
