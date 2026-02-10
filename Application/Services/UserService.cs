using Application.DTOs;
using Application.Services.Interfaces;
using Domain.Entities;
using Domain.Models;
using Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
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
                Email = request.Email,
                PasswordHash = request.Password != null ? HashPassword(request.Password) : string.Empty,
                Role = request.Role ?? Domain.Enums.Role.Student
            };

            var response = await _userRepository.Create(user);
            await unitOfWork.SaveChanges();

            return new UserResponse(
                response.Id,
                response.Name,
                response.Email,
                response.Role
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
                user.Email,
                user.Role
            );
        }

        public async Task<List<UserResponse>> GetAllAsync()
        {
            var users = await _userRepository.GetAllAsync();

            return users
                .Select(u => new UserResponse(
                    u.Id,
                    u.Name,
                    u.Email,
                    u.Role
                ))
                .ToList();
        }

        private string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(hashedBytes);
        }
    }
}
