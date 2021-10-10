using PaymentSystem.Repo.Dto;
using PaymentSystem.Repo.Entities;
using PaymentSystem.Repo.Interfaces;
using System;
using System.Linq;

namespace PaymentSystem.Repo
{
    public class UserRepo : IUserRepo
    {
        private readonly EntityDBContex _context;

        public UserRepo(EntityDBContex context)
        {
            _context = context;
        }

        public UserDto GetUser(string UserName)
        {
            var user = _context.Users.FirstOrDefault(f => f.Username == UserName);
            if (user == null)
                return null;
            return new UserDto
            {
                UserId = user.UserId,
                Username = user.Username
            };
        }

        public UserDto CreateUser(string UserName)
        {
            var UserId = Guid.NewGuid();
            var user = new User
            {
                UserId = UserId,
                Username = UserName,
                LastLogin = DateTime.Now
            };

            var userNew = _context.Add(user);
            _context.SaveChanges();

            return new UserDto
            {
                UserId = userNew.Entity.UserId,
                Username = userNew.Entity.Username
            };
        }
    }
}
