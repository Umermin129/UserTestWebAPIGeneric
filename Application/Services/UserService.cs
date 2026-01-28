using Application.DTOs;
using Application.Services.Interfaces;
using Domain.Entities;
using Domain.Models;
using Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Services
{
    public class UserService(
        IUnitOfWork unitOfWork,
        IRepository<User> userRepository
    ) : IService<User, UserResponse, UserModel>
    {
        private readonly IRepository<User> _userRepository = userRepository;

        public async Task<UserResponse> CreateAsync(UserModel request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            var user = new User
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                Email = request.Email
            };

            var response = await _userRepository.Create(user);
            await unitOfWork.SaveChanges();

            return new UserResponse(
                response.Id,
                response.Name,
                response.Email
            );
        }

        public async Task<UserResponse> GetAsync(Guid id)
        {
            var user = await _userRepository.GetAsync(id);
            if (user == null)
                throw new ApplicationException("User not found");

            return new UserResponse(
                user.Id,
                user.Name,
                user.Email
            );
        }

        public async Task<List<UserResponse>> GetAllAsync()
        {
            var users = await _userRepository.GetAllAsync();

            return users
                .Select(u => new UserResponse(
                    u.Id,
                    u.Name,
                    u.Email
                ))
                .ToList();
        }
    }
}
