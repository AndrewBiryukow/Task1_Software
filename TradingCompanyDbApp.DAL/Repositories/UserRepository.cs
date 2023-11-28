using TradingCompanyDbApp.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using TradingCompanyDbApp.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradingCompanyDbApp.DAL.Contexts;
using TradingCompanyDbApp.DTO.ModelsDTO;

namespace TradingCompanyDbApp.DAL.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly TradeDbContext _dbContext;

        public UserRepository(TradeDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<UserDTO> GetUserByIdAsync(int userId)
        {
            return await _dbContext.Users.FindAsync(userId);
        }

        public async Task<List<UserDTO>> GetAllUsersAsync()
        {
            return await _dbContext.Users.ToListAsync();
        }

        public async Task<UserDTO> CreateUserAsync(UserDTO user)
        {
            _dbContext.Users.Add(user);
            await _dbContext.SaveChangesAsync();
            return user;
        }

        public async Task UpdateUserAsync(UserDTO user)
        {
            _dbContext.Users.Update(user);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteUserAsync(int userId)
        {
            var user = await _dbContext.Users.FindAsync(userId);

            if (user != null)
            {
                _dbContext.Users.Remove(user);
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task<UserDTO> GetUserByNickname(string nickname)
        {
            return await _dbContext.Users.FirstOrDefaultAsync(u => u.Nickname == nickname);
        }
    }
}
