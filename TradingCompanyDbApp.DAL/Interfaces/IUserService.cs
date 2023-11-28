using System.Collections.Generic;
using System.Threading.Tasks;
using TradingCompanyDbApp.DTO.ModelsDTO;

namespace TradingCompanyDbApp.DAL.Interfaces
{
    public interface IUserService
    {
        Task<List<UserDTO>> GetAllUsersAsync();
        Task<UserDTO> GetUserByIdAsync(int userId);
        Task<UserDTO> CreateUserAsync(UserDTO userDTO);
        Task UpdateUserAsync(UserDTO userDTO);
        Task DeleteUserAsync(int userId);
        Task<UserDTO> GetUserByNicknameAsync(string nickname);
    }
}
