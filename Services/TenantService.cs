using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Models;
using DTOs;
using cks_kaas.Data;

namespace Services
{
    public class TenantService
    {
        private readonly AppDbContext _db;
        public TenantService(AppDbContext db)
        {
            _db = db;
        }

        public async Task<(List<TenantDto> Tenants, int Total)> ListTenantsAsync(int page = 1, int pageSize = 20, string? search = null)
        {
            var query = _db.Tenants.AsQueryable();
            if (!string.IsNullOrEmpty(search))
                query = query.Where(t => t.Name.Contains(search) || t.AdminEmail.Contains(search));
            var total = await query.CountAsync();
            var tenants = await query.OrderBy(t => t.Name).Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
            return (tenants.Select(ToDto).ToList(), total);
        }

        public async Task<TenantDto?> GetTenantAsync(Guid tenantId)
        {
            var tenant = await _db.Tenants.FirstOrDefaultAsync(t => t.Id == tenantId);
            return tenant == null ? null : ToDto(tenant);
        }

        public async Task<TenantDto> CreateTenantAsync(CreateTenantDto dto)
        {
            var tenant = new Tenant
            {
                Id = Guid.NewGuid(),
                Name = dto.Name,
                AdminEmail = dto.AdminEmail,
                CreatedAt = DateTime.UtcNow
            };
            _db.Tenants.Add(tenant);
            await _db.SaveChangesAsync();
            return ToDto(tenant);
        }

        public static TenantDto ToDto(Tenant tenant) => new TenantDto
        {
            Id = tenant.Id,
            Name = tenant.Name,
            AdminEmail = tenant.AdminEmail,
            CreatedAt = tenant.CreatedAt,
            UpdatedAt = tenant.CreatedAt,
            Status = "active"
        };
    }
}
