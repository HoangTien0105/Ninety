using Ninety.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ninety.Data.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<User> GetAccount(string email, string password);
        Task<List<User>> GetAll();
        Task<User> GetById(int id);
        Task<User> GetByEmail(string email);
        Task<User> GetByPhone(string phone);
        Task<User> GetByName(string name);
        Task<User> Add(User user);
        Task<User> Update(User user);   
    }
}
