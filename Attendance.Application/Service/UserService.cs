using AttendanceTracker.Application.DTOs;
using AttendanceTracker.Application.Interfaces;
using AttendenceTracker.Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AttendanceTracker.Application.Services
{
    public class UserService : IUserService
    {
        private readonly AttendanceDbContext _context;
        private readonly IConfiguration _config;

        public UserService(AttendanceDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        public async Task<IEnumerable<UserResponseDTO>> GetAllAsync()
        {
            var users = await _context.Users
                .Include(u => u.Role)
                .Include(u => u.UserDetails)
                .ToListAsync();

            return users.Select(MapUserToResponse).ToList();
        }

        public async Task<UserResponseDTO?> GetByIdAsync(int id)
        {
            var user = await _context.Users
                .Include(u => u.Role)
                .Include(u => u.UserDetails)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (user == null) return null;

            return MapUserToResponse(user);
        }

        public async Task<UserResponseDTO> CreateAsync(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            var createdUser = await _context.Users
                .Include(u => u.Role)
                .Include(u => u.UserDetails)
                .FirstAsync(u => u.Id == user.Id);

            return MapUserToResponse(createdUser);
        }

        public async Task<UserResponseDTO?> UpdateAsync(int id, User user, string? password = null)
        {
            var existing = await _context.Users.FindAsync(id);
            if (existing == null) return null;

            existing.UserName = user.UserName;
            existing.Email = user.Email;
            existing.RoleID = user.RoleID;

            if (!string.IsNullOrWhiteSpace(password))
            {
                existing.PasswordHash = BCrypt.Net.BCrypt.HashPassword(password);
            }

            await _context.SaveChangesAsync();

            var updatedUser = await _context.Users
                .Include(u => u.Role)
                .Include(u => u.UserDetails)
                .FirstAsync(u => u.Id == id);

            return MapUserToResponse(updatedUser);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return false;

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<LoginResponseDTO?> LoginAsync(string username, string password)
        {
            var user = await _context.Users
                .Include(u => u.Role)
                .Include(u => u.UserDetails)
                .FirstOrDefaultAsync(x => x.UserName == username);

            if (user == null) return null;

            bool isPasswordValid = false;

            try
            {
                isPasswordValid = BCrypt.Net.BCrypt.Verify(password, user.PasswordHash);
            }
            catch
            {
                return null;
            }

            if (!isPasswordValid)
                return null;

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Role, user.Role?.RoleName ?? "")
            };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: creds);

            return new LoginResponseDTO
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                User = MapUserToResponse(user)
            };
        }

        private static UserResponseDTO MapUserToResponse(User user)
        {
            return new UserResponseDTO
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                RoleID = user.RoleID,
                RoleName = user.Role?.RoleName ?? "",
                CreatedAt = user.CreatedAt,
                UserDetails = user.UserDetails.Select(d => new UserDetailsDTO
                {
                    UserID = d.UserID,
                    FullName = d.FullName,
                    DOB = d.DOB,
                    Gender = d.Gender,
                    PhoneNumber = d.PhoneNumber,
                    Address = d.Address,
                    Department = d.Department,
                    Year = d.Year
                }).ToList()
            };
        }
    }
}