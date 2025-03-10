using Microsoft.AspNetCore.Mvc;
using pd311_web_api.BLL.Services.Account;
using pd311_web_api.BLL.DTOs.Account;
using pd311_web_api.BLL.DTOs.User;

using Microsoft.AspNetCore.Identity;

using static pd311_web_api.DAL.Entities.IdentityEntities;

namespace pd311_web_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly UserManager<AppUser> _userManager;

        public UserController(IAccountService accountService, UserManager<AppUser> userManager)
        {
            _accountService = accountService;
            _userManager = userManager;
        }

        // Створення користувача
        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] RegisterDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest("Невірні дані.");

            // Перевірка на наявність користувача з таким email або username
            if (await _userManager.FindByEmailAsync(dto.Email) != null)
                return Conflict("Email вже зайнятий.");
            if (await _userManager.FindByNameAsync(dto.UserName) != null)
                return Conflict("Ім'я користувача вже зайняте.");

            var result = await _accountService.RegisterAsync(dto);
            if (result.Succeeded)
                return Ok("Користувача створено успішно.");

            return BadRequest(result.Errors.FirstOrDefault()?.Description);
        }

        // Оновлення користувача
        [HttpPut("update/{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] UpdateUserDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest("Невірні дані.");

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
                return NotFound("Користувача не знайдено.");

            // Оновлення дозволених полів
            user.Email = dto.Email ?? user.Email;
            user.UserName = dto.UserName ?? user.UserName;

            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
                return Ok("Користувача оновлено успішно.");

            return BadRequest(result.Errors.FirstOrDefault()?.Description);
        }

        // Видалення користувача
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
                return NotFound("Користувача не знайдено.");

            var result = await _userManager.DeleteAsync(user);
            if (result.Succeeded)
                return Ok("Користувача видалено успішно.");

            return BadRequest(result.Errors.FirstOrDefault()?.Description);
        }

        // Отримати користувача за ID
        [HttpGet("getById/{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
                return NotFound("Користувача не знайдено.");

            var userDto = new UserDto
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email
            };

            return Ok(userDto);
        }

        // Отримати всіх користувачів
        [HttpGet("getAll")]
        public async Task<IActionResult> GetAll()
        {
            var users = _userManager.Users.ToList();
            if (!users.Any())
                return NotFound("Користувачі не знайдені.");

            var userDtos = users.Select(user => new UserDto
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email
            }).ToList();

            return Ok(userDtos);
        }
    }
}
