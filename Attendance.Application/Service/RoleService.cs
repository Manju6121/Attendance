using AttendanceTracker.Application.Interfaces;
using AttendenceTracker.Domain.Entity;
using Microsoft.EntityFrameworkCore;

namespace AttendanceTracker.Application.Services
{
    public class RoleService : IRoleService
    {
        private readonly AttendanceDbContext _context;

        public RoleService(AttendanceDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Role>> GetAllAsync()
        {
            try
            {
                return await _context.Roles.ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Role?> GetByIdAsync(int id)
        {
            try
            {
                return await _context.Roles.FindAsync(id);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Role> CreateAsync(Role role)
        {
            try
            {
                _context.Roles.Add(role);
                await _context.SaveChangesAsync();
                return role;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Role?> UpdateAsync(int id, Role role)
        {
            try
            {
                var existing = await _context.Roles.FindAsync(id);
                if (existing == null) return null;

                existing.RoleName = role.RoleName;
                existing.Description = role.Description;

                await _context.SaveChangesAsync();
                return existing;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var role = await _context.Roles.FindAsync(id);
                if (role == null) return false;

                _context.Roles.Remove(role);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}