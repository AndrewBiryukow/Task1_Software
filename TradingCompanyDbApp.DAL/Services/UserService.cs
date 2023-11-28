using AutoMapper;
using TradingCompanyDbApp.DAL.Interfaces;
using TradingCompanyDbApp.DAL.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TradingCompanyDbApp.DTO.ModelsDTO;

namespace TradingCompanyDbApp.DAL.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<User, UserDTO>();
                cfg.CreateMap<UserDTO, User>(); 
                cfg.CreateMap<UserDTO, UserDTO>(); 
                
            });

            _mapper = new Mapper(config);
        }

        public async Task<List<UserDTO>> GetAllUsersAsync()
        {
            var users = await _userRepository.GetAllUsersAsync();
            return _mapper.Map<List<UserDTO>>(users);
        }

        public async Task<UserDTO> GetUserByIdAsync(int userId)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);
            return _mapper.Map<UserDTO>(user);
        }

        public async Task<UserDTO> CreateUserAsync(UserDTO userDTO)
        {
            UserDTO user = _mapper.Map<UserDTO>(userDTO);
            UserDTO model = await _userRepository.CreateUserAsync(user);
            return model;
        }

        public async Task UpdateUserAsync(UserDTO userDTO)
        {
            var existingUser = await _userRepository.GetUserByIdAsync(userDTO.Id);

            if (existingUser == null)
            {
                throw new Exception("User not found"); 
            }

            
            _mapper.Map(userDTO, existingUser);

            await _userRepository.UpdateUserAsync(existingUser);
        }

        public async Task<UserDTO> GetUserByNicknameAsync(string nickname)
        {
            return await _userRepository.GetUserByNickname(nickname);
        }
        public async Task DeleteUserAsync(int userId)
        {
            var existingUser = await _userRepository.GetUserByIdAsync(userId);

            if (existingUser == null)
            {
                throw new Exception("User not found");
            }

            await _userRepository.DeleteUserAsync(userId);
        }
    }
}
