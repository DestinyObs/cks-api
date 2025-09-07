using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services;
using DTOs;

namespace cks_kaas.Controllers
{
    [ApiController]
    [Route("api/tenants/{tenantId:guid}/users")]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;
        public UserController(UserService userService)
        {
            _userService = userService;
        }

        [HttpGet("list")]
        public async Task<IActionResult> ListUsers(Guid tenantId, [FromQuery] int page = 1, [FromQuery] int pageSize = 20, [FromQuery] string? search = null, [FromQuery] string? role = null)
        {
            var (users, total) = await _userService.ListUsersAsync(tenantId, page, pageSize, search, role);
            return Ok(new { data = new { users, total, page, pageSize } });
        }

        [HttpGet("get/{id:guid}")]
        public async Task<IActionResult> GetUser(Guid tenantId, Guid id)
        {
            var user = await _userService.GetUserAsync(tenantId, id);
            if (user == null) return NotFound();
            return Ok(new { data = user });
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateUser(Guid tenantId, [FromBody] CreateUserDto dto)
        {
            var user = await _userService.CreateUserAsync(tenantId, dto);
            return Ok(new { data = user });
        }

        [HttpPut("update/{id:guid}")]
        public async Task<IActionResult> UpdateUser(Guid tenantId, Guid id, [FromBody] UpdateUserDto dto)
        {
            var user = await _userService.UpdateUserAsync(tenantId, id, dto);
            if (user == null) return NotFound();
            return Ok(new { data = user });
        }

        [HttpDelete("delete/{id:guid}")]
        public async Task<IActionResult> DeleteUser(Guid tenantId, Guid id)
        {
            var result = await _userService.DeleteUserAsync(tenantId, id);
            if (!result) return NotFound();
            return Ok(new { data = new { } });
        }

        [HttpPost("suspend/{id:guid}")]
        public async Task<IActionResult> SuspendUser(Guid tenantId, Guid id)
        {
            var result = await _userService.SuspendUserAsync(tenantId, id);
            if (!result) return NotFound();
            return Ok(new { data = new { } });
        }

        [HttpPost("{id:guid}/activate")]
        public async Task<IActionResult> ActivateUser(Guid tenantId, Guid id)
        {
            var result = await _userService.ActivateUserAsync(tenantId, id);
            if (!result) return NotFound();
            return Ok(new { data = new { } });
        }

        [HttpPost("{id:guid}/reset-password")]
        public async Task<IActionResult> ResetPassword(Guid tenantId, Guid id)
        {
            var token = await _userService.ResetPasswordAsync(tenantId, id);
            if (token == null) return NotFound();
            return Ok(new { data = new { resetToken = token } });
        }
    }
}
