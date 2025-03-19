using LuanAnTotNghiep_TuanVu_TuBac.Models.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LuanAnTotNghiep_TuanVu_TuBac.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetAllUsers();
        Task<User> GetUserById(int id);
        Task<User> GetUserByEmail(string email);
        Task<User?> GetUserByPhoneNumber(string phoneNumber);
        Task AddUser(User user);
        Task<bool> UpdateUser(User user);
        Task DeleteUser(int id);

        Task UpdateUserAsync(User user);
    }
}
