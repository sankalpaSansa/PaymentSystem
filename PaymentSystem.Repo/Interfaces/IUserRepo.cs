using PaymentSystem.Repo.Dto;

namespace PaymentSystem.Repo.Interfaces
{
    public interface IUserRepo
    {
        UserDto GetUser(string UserName);
        UserDto CreateUser(string UserName);
    }
}
