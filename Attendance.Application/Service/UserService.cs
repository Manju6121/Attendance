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
            try
            {
                var users = await _context.Users
                    .Include(u => u.Role)
                    .Include(u => u.UserDetails)
                    .ToListAsync();

                return users.Select(MapUserToResponse).ToList();
            }
            catch
            {
                throw;
            }
        }

        public async Task<UserResponseDTO?> GetByIdAsync(int id)
        {
            try
            {
                var user = await _context.Users
                    .Include(u => u.Role)
                    .Include(u => u.UserDetails)
                    .FirstOrDefaultAsync(x => x.Id == id);

                if (user == null) return null;

                return MapUserToResponse(user);
            }
            catch
            {
                throw;
            }
        }

        public async Task<UserResponseDTO> CreateAsync(User user)
        {
            try
            {
                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                var createdUser = await _context.Users
                    .Include(u => u.Role)
                    .Include(u => u.UserDetails)
                    .FirstAsync(x => x.Id == user.Id);

                return MapUserToResponse(createdUser);
            }
            catch
            {
                throw;
            }
        }

        public async Task<UserResponseDTO?> UpdateAsync(int id, User user, string? password = null)
        {
            try
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
                    .FirstAsync(x => x.Id == id);

                return MapUserToResponse(updatedUser);
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var user = await _context.Users.FindAsync(id);
                if (user == null) return false;

                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                throw;
            }
        }

        public async Task<LoginResponseDTO?> LoginAsync(string username, string password)
        {
            try
            {
                var user = await _context.Users
                    .Include(u => u.Role)
                    .Include(u => u.UserDetails)
                    .FirstOrDefaultAsync(x => x.UserName == username);

                if (user == null) return null;

                bool isValid = BCrypt.Net.BCrypt.Verify(password, user.PasswordHash);
                if (!isValid) return null;

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
            catch
            {
                throw;
            }
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