using Application.DTOs;
using Application.Services.Interfaces;
using Application.Interfaces;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;

namespace Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public async Task<CreateUserResponse> CreateUser(UserModel request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }
            else
            {
                var createUser = new User { Id = Guid.NewGuid(), Name = request.Name, Email = request.Email };
                var response = await _userRepository.CreateUser(createUser);
                if (response == null)
                {
                    throw new ApplicationException("User Creation was failed");
                }
                var userName = new CreateUserResponse { Name = response.Name };
                return userName;
            }
        }
        //public async Task<UserResponse> GetUserById(Guid id)
        //{

        //}
    }
}
