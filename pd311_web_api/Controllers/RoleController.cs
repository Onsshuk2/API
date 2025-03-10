using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using pd311_web_api.BLL.DTOs.Role;
using pd311_web_api.BLL.Services.Role;

namespace pd311_web_api.Controllers
{
    [ApiController]
    [Route("api/role")]
    public class RoleController : BaseController
    {
        private readonly IRoleService _roleService;
        private readonly IValidator<RoleDto> _roleValidator;

        public RoleController(IRoleService roleService, IValidator<RoleDto> roleValidator)
        {
            _roleService = roleService;
            _roleValidator = roleValidator;
        }

        [HttpGet("list")]
        public async Task<IActionResult> GetAllAsync()
        {
            var roles = await _roleService.GetAllAsync();
            return Ok(roles);
        }

        [HttpGet]
        public async Task<IActionResult> GetByIdAsync(string? id)
        {
            if (!ValidateId(id, out string message))
                return BadRequest(message);

            var result = await _roleService.GetByIdAsync(id);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] RoleDto dto)
        {
            var validationResult = await _roleValidator.ValidateAsync(dto);

            if (!validationResult.IsValid)
                return BadRequest(validationResult);

            var result = await _roleService.CreateAsync(dto);
            return result ? Ok("Role created") : BadRequest("Role not created");
        }

        [HttpPut]
        public async Task<IActionResult> UpdateAsync([FromBody] RoleDto dto)
        {
            var validationResult = await _roleValidator.ValidateAsync(dto);

            if (!validationResult.IsValid)
                return BadRequest(validationResult);

            if (!ValidateId(dto.Id, out string message))
                return BadRequest(message);

            var result = await _roleService.UpdateAsync(dto);
            return result ? Ok("Role updated") : BadRequest("Role not updated");
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteAsync(string? id) 
        {
            if (!ValidateId(id, out string message))
                return BadRequest(message);

            var result = await _roleService.DeleteAsync(id);
            return result ? Ok("Role deleted") : BadRequest("Role not deleted");
        }
    }
}
