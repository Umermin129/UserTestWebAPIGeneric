using Application.DTOs;
using Application.Services.Interfaces;
using Domain.Constants;
using Domain.Entities;
using Domain.Enums;
using Infrastructure.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IRepository<User> _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;

        public AuthService(
            IRepository<User> userRepository,
            IUnitOfWork unitOfWork,
            IConfiguration configuration)
        {
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
            _configuration = configuration;
        }

        public async Task<AuthResponse> RegisterAsync(RegisterRequest request)
        {
            var existingUsers = await _userRepository.GetAllAsync();
            if (existingUsers.Any(u => u.Email == request.Email))
            {
                throw new ApplicationException("User with this email already exists");
            }
            var passwordHash = HashPassword(request.Password);
            var user = new User
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                Email = request.Email,
                PasswordHash = passwordHash,
                Role = request.Role
            };
            await _userRepository.Create(user);
            await _unitOfWork.SaveChanges();
            var token = GenerateJwtToken(user);
            var permissions = RolePermissions.GetPermissionNamesForRole(user.Role);
            
            return new AuthResponse(
                Token: token,
                Email: user.Email,
                Name: user.Name,
                UserId: user.Id,
                Role: user.Role,
                Permissions: permissions
            );
        }

        public async Task<AuthResponse> LoginAsync(LoginRequest request)
        {
            var users = await _userRepository.GetAllAsync();
            var user = users.FirstOrDefault(u => u.Email == request.Email);
            
            if (user == null)
            {
                throw new ApplicationException("Invalid email or password");
            }
            
            if (!VerifyPassword(request.Password, user.PasswordHash))
            {
                throw new ApplicationException("Invalid email or password");
            }

            var token = GenerateJwtToken(user);
            var permissions = RolePermissions.GetPermissionNamesForRole(user.Role);
            
            return new AuthResponse(
                Token: token,
                Email: user.Email,
                Name: user.Name,
                UserId: user.Id,
                Role: user.Role,
                Permissions: permissions
            );
        }

        private string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(hashedBytes);
        }

        private bool VerifyPassword(string password, string passwordHash)
        {
            var hashOfInput = HashPassword(password);
            return hashOfInput == passwordHash;
        }

        private string GenerateJwtToken(User user)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var secretKey = jwtSettings["SecretKey"] ?? "abcdabcdabcdbacdbabcdabcdabcdabcdabcdabcd";
            var issuer = jwtSettings["Issuer"] ?? "UsertestWebAPI";
            var audience = jwtSettings["Audience"] ?? "UsertestWebAPI";
            var expiryMinutes = int.Parse(jwtSettings["ExpiryMinutes"] ?? "60");

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var permissions = RolePermissions.GetPermissionNamesForRole(user.Role);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Role, user.Role.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            foreach (var permission in permissions)
            {
                claims.Add(new Claim("Permission", permission));
            }

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(expiryMinutes),
                signingCredentials: credentials);
            
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
