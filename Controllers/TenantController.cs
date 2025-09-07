using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services;
using DTOs;

namespace Controllers
{
    [ApiController]
    [Route("api/provider/tenants")]
    [Authorize(Roles = "ProviderAdmin")]
    public class TenantController : ControllerBase
    {
        private readonly TenantService _tenantService;
        public TenantController(TenantService tenantService)
        {
            _tenantService = tenantService;
        }

        [HttpGet]
        public async Task<IActionResult> ListTenants([FromQuery] int page = 1, [FromQuery] int pageSize = 20, [FromQuery] string? search = null)
        {
            var (tenants, total) = await _tenantService.ListTenantsAsync(page, pageSize, search);
            return Ok(new { data = new { tenants, total, page, pageSize } });
        }

        [HttpGet("{tenantId:guid}")]
        public async Task<IActionResult> GetTenant(Guid tenantId)
        {
            var tenant = await _tenantService.GetTenantAsync(tenantId);
            if (tenant == null) return NotFound();
            return Ok(new { data = tenant });
        }

        [HttpPost]
        public async Task<IActionResult> CreateTenant([FromBody] CreateTenantDto dto)
        {
            var tenant = await _tenantService.CreateTenantAsync(dto);
            return Ok(new { data = tenant });
        }
    }
}
