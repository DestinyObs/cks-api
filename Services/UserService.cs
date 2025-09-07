using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Models;
using DTOs;
using cks_kaas.Data;

namespace Services
{
    public class UserService
    {
        private readonly AppDbContext _db;
        private readonly UserManager<User> _userManager;
        public UserService(AppDbContext db, UserManager<User> userManager)
        {
            _db = db;
            _userManager = userManager;
        }

        public async Task<(List<UserDto> Users, int Total)> ListUsersAsync(Guid tenantId, int page = 1, int pageSize = 20, string? search = null, string? role = null)
        {
            var query = _db.Users.Where(u => u.TenantId == tenantId);
            if (!string.IsNullOrEmpty(search))
                query = query.Where(u => u.Name.Contains(search) || u.Email.Contains(search));
            if (!string.IsNullOrEmpty(role))
                query = query.Where(u => u.Role == role);
            var total = await query.CountAsync();
            var users = await query.OrderBy(u => u.Name).Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
            return (users.Select(ToDto).ToList(), total);
        }

        public async Task<UserDto?> GetUserAsync(Guid tenantId, Guid userId)
        {
            var user = await _db.Users.FirstOrDefaultAsync(u => u.TenantId == tenantId && u.Id == userId);
            return user == null ? null : ToDto(user);
        }

        public async Task<UserDto> CreateUserAsync(Guid tenantId, CreateUserDto dto)
        {
            var user = new User
            {
                Id = Guid.NewGuid(),
                TenantId = tenantId,
                Email = dto.Email,
                UserName = dto.Email,
                Name = dto.Name,
                Role = dto.Role,
                Avatar = dto.Avatar,
                Status = "active",
                JoinDate = DateTime.UtcNow,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
            var result = await _userManager.CreateAsync(user, dto.Password);
            if (!result.Succeeded)
                throw new Exception(string.Join("; ", result.Errors.Select(e => e.Description)));
            return ToDto(user);
        }

        public async Task<UserDto?> UpdateUserAsync(Guid tenantId, Guid userId, UpdateUserDto dto)
        {
            var user = await _db.Users.FirstOrDefaultAsync(u => u.TenantId == tenantId && u.Id == userId);
            if (user == null) return null;
            if (dto.Name != null) user.Name = dto.Name;
            if (dto.Role != null) user.Role = dto.Role;
            if (dto.Avatar != null) user.Avatar = dto.Avatar;
            user.UpdatedAt = DateTime.UtcNow;
            await _db.SaveChangesAsync();
            return ToDto(user);
        }

        public async Task<bool> DeleteUserAsync(Guid tenantId, Guid userId)
        {
            var user = await _db.Users.FirstOrDefaultAsync(u => u.TenantId == tenantId && u.Id == userId);
            if (user == null) return false;
            _db.Users.Remove(user);
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> SuspendUserAsync(Guid tenantId, Guid userId)
        {
            var user = await _db.Users.FirstOrDefaultAsync(u => u.TenantId == tenantId && u.Id == userId);
            if (user == null) return false;
            user.Status = "suspended";
            user.UpdatedAt = DateTime.UtcNow;
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ActivateUserAsync(Guid tenantId, Guid userId)
        {
            var user = await _db.Users.FirstOrDefaultAsync(u => u.TenantId == tenantId && u.Id == userId);
            if (user == null) return false;
            user.Status = "active";
            user.UpdatedAt = DateTime.UtcNow;
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<string?> ResetPasswordAsync(Guid tenantId, Guid userId)
        {
            var user = await _db.Users.FirstOrDefaultAsync(u => u.TenantId == tenantId && u.Id == userId);
            if (user == null) return null;
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            return token;
        }

        public static UserDto ToDto(User user) => new UserDto
        {
            Id = user.Id,
            TenantId = user.TenantId,
            Email = user.Email!,
            Name = user.Name,
            Avatar = user.Avatar,
            Role = user.Role,
            Status = user.Status,
            LastLogin = user.LastLogin,
            JoinDate = user.JoinDate,
            CreatedAt = user.CreatedAt,
            UpdatedAt = user.UpdatedAt
        };
    }
}
