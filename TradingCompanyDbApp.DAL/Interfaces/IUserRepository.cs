using TradingCompanyDbApp.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradingCompanyDbApp.DTO.ModelsDTO;

namespace TradingCompanyDbApp.DAL.Interfaces
{
    public interface IUserRepository
    {
        Task<UserDTO> GetUserByIdAsync(int userId);
        Task<List<UserDTO>> GetAllUsersAsync();
        Task<UserDTO> CreateUserAsync(UserDTO user);
        Task UpdateUserAsync(UserDTO user);
        Task DeleteUserAsync(int userId);

        Task<UserDTO> GetUserByNickname(string nickname);
    }
}
