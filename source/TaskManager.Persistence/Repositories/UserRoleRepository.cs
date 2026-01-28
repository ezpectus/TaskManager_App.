using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Interfaces;
using TaskManager.Persistence.Contexts;





namespace TaskManager.Persistence.Repositories;

    public class UserRoleRepository : IUserRoleRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRoleRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<UserRole?> GetUserRoleAsync(Guid userId, Guid roleId)
        {
            return await _context.UserRoles
                .Include(x => x.Role)
                .Include(x => x.User)
                .FirstOrDefaultAsync(x => x.UserId == userId && x.RoleId == roleId);
        }

        public async Task<IEnumerable<UserRole>> GetByUserIdAsync(Guid userId)
        {
            return await _context.UserRoles
                .Include(x => x.Role)
                .Where(x => x.UserId == userId)
                .ToListAsync();
        }

        public async Task<IEnumerable<UserRole>> GetByRoleIdAsync(Guid roleId)
        {
            return await _context.UserRoles
                .Include(x => x.User)
                .Where(x => x.RoleId == roleId)
                .ToListAsync();
        }

        public async Task AddAsync(UserRole userRole)
        {
            await _context.UserRoles.AddAsync(userRole);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveAsync(UserRole userRole)
        {
            _context.UserRoles.Remove(userRole);
            await _context.SaveChangesAsync();
        }

        public async Task AssignRoleToUserAsync(Guid userId, Guid roleId)
        {
            var existing = await GetUserRoleAsync(userId, roleId);
            if (existing == null)
            {
                await AddAsync(new UserRole { UserId = userId, RoleId = roleId });
            }
        }

        public async Task RemoveRoleFromUserAsync(Guid userId, Guid roleId)
        {
            var role = await GetUserRoleAsync(userId, roleId);
            if (role != null)
            {
                await RemoveAsync(role);
            }
        }
    }
