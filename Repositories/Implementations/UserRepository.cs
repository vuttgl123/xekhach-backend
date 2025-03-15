using LuanAnTotNghiep_TuanVu_TuBac.Data;
using LuanAnTotNghiep_TuanVu_TuBac.Models.Entities;
using LuanAnTotNghiep_TuanVu_TuBac.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LuanAnTotNghiep_TuanVu_TuBac.Repositories.Implementations
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Lấy danh sách tất cả người dùng
        /// </summary>
        public async Task<IEnumerable<User>> GetAllUsers()
        {
            return await _context.Users.AsNoTracking().ToListAsync();
        }

        /// <summary>
        /// Tìm người dùng theo ID
        /// </summary>
        public async Task<User?> GetUserById(int id)
        {
            return await _context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Id == id);
        }

        /// <summary>
        /// Tìm người dùng theo Email
        /// </summary>
        public async Task<User?> GetUserByEmail(string email)
        {
            return await _context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<User?> GetUserByPhoneNumber(string phoneNumber)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.PhoneNumber == phoneNumber);
        }


        /// <summary>
        /// Thêm người dùng mới
        /// </summary>
        public async Task AddUser(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Cập nhật thông tin người dùng
        /// </summary>
        public async Task UpdateUser(User user)
        {
            var existingUser = await _context.Users.FindAsync(user.Id);
            if (existingUser == null)
            {
                throw new InvalidOperationException($"User với ID {user.Id} không tồn tại.");
            }

            // Cập nhật chỉ các trường cần thiết
            existingUser.Email = user.Email;
            existingUser.PasswordHash = user.PasswordHash;
            existingUser.FullName = user.FullName;
            existingUser.Gender = user.Gender;
            existingUser.PhoneNumber = user.PhoneNumber;
            existingUser.Address = user.Address;
            existingUser.UpdatedAt = DateTime.UtcNow;
            existingUser.IsActive = user.IsActive;
            existingUser.Role = user.Role;
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Xóa người dùng theo ID
        /// </summary>
        public async Task DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                throw new InvalidOperationException($"User với ID {id} không tồn tại.");
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateUserAsync(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }
    }
}
