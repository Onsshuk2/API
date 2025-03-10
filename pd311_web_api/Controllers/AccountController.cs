using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using pd311_web_api.BLL.DTOs.Account;
using pd311_web_api.BLL.Services.Account;

namespace pd311_web_api.Controllers
{
    [ApiController]
    [Route("api/account")]
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;
        private readonly IValidator<LoginDto> _loginValidator;
        private readonly IValidator<RegisterDto> _registerValidator;

        public AccountController(
            IAccountService accountService,
            IValidator<LoginDto> loginValidator,
            IValidator<RegisterDto> registerValidator)
        {
            _accountService = accountService;
            _loginValidator = loginValidator;
            _registerValidator = registerValidator;
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync([FromBody] LoginDto dto)
        {
            var validationResult = await _loginValidator.ValidateAsync(dto);
            if (!validationResult.IsValid)
                return BadRequest(validationResult);

            var user = await _accountService.LoginAsync(dto);
            return user == null ? BadRequest("Incorrect username or password") : Ok(user);
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterAsync([FromBody] RegisterDto dto)
        {
            var validationResult = await _registerValidator.ValidateAsync(dto);
            if (!validationResult.IsValid)
                return BadRequest(validationResult);

            var result = await _accountService.RegisterAsync(dto);

            if (!result.Succeeded)
                return BadRequest(result.Errors.Select(e => e.Description));

            return Ok("Registration successful! Check your email to confirm your account.");
        }

        [HttpGet("confirmEmail")]
        public async Task<IActionResult> ConfirmEmailAsync(string? id, string? t)
        {
            if (string.IsNullOrEmpty(id) || string.IsNullOrEmpty(t))
                return NotFound();

            var result = await _accountService.ConfirmEmailAsync(id, t);

            return result ? Ok("Email confirmed successfully!") : BadRequest("Invalid confirmation link.");
        }
    }
}
