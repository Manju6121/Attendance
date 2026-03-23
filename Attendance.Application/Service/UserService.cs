using AttendanceTracker.Application.DTOs;
using AttendanceTracker.Application.Interfaces;
using AttendenceTracker.Domain.Entity;
using AutoMapper;
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
        private readonly IMapper _mapper;

        public UserService(AttendanceDbContext context, IConfiguration config, IMapper mapper)
        {
            _context = context;
            _config = config;
            _mapper = mapper;
        }

        public async Task<IEnumerable<UserResponseDTO>> GetAllAsync()
        {
            var users = await _context.Users
                .Include(u => u.Role)
                .Include(u => u.UserDetails)
                .ToListAsync();

            var dtoList = _mapper.Map<List<UserResponseDTO>>(users);

            return dtoList;
        }

        public async Task<UserResponseDTO?> GetByIdAsync(int id)
        {
            var user = await _context.Users
                .Include(u => u.Role)
                .Include(u => u.UserDetails)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (user == null) return null;

            var dto = _mapper.Map<UserResponseDTO>(user);

            return dto;
        }

        public async Task<UserResponseDTO> CreateAsync(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            var createdUser = await _context.Users
                .Include(u => u.Role)
                .Include(u => u.UserDetails)
                .FirstAsync(x => x.Id == user.Id);

            return _mapper.Map<UserResponseDTO>(createdUser);
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
                .FirstAsync(x => x.Id == id);

            return _mapper.Map<UserResponseDTO>(updatedUser);
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
                User = _mapper.Map<UserResponseDTO>(user)
            };
        }
    }
}