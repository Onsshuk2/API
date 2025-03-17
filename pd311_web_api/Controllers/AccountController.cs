using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using pd311_web_api.BLL.DTOs.Account;
using pd311_web_api.BLL.Services.Account;

namespace pd311_web_api.Controllers
{
    [ApiController]
    [Route("api/account")]
    public class AccountController : BaseController
    {
        private readonly IAccountService _accountService;
        private readonly IValidator<LoginDto> _loginValidator;

        public AccountController(IAccountService accountService, IValidator<LoginDto> loginValidator)
        {
            _accountService = accountService;
            _loginValidator = loginValidator;
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync([FromBody] LoginDto dto)
        {
            // validation >>>
            var validationResult = await _loginValidator.ValidateAsync(dto);

            if (!validationResult.IsValid)
                return BadRequest(validationResult);
            // <<< end

            var response = await _accountService.LoginAsync(dto);

            return CreateActionResult(response);
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterAsync([FromBody] RegisterDto dto)
        {
            var user = await _accountService.RegisterAsync(dto);

            return user == null ? BadRequest("Register error") : Ok(user);
        }

        [HttpGet("confirmEmail")]
        public async Task<IActionResult> ConfirmEmailAsync(string? id, string? t)
        {
            if (string.IsNullOrEmpty(id) || string.IsNullOrEmpty(t))
                return NotFound();

            await _accountService.ConfirmEmailAsync(id, t);

            return Redirect("https://google.com");
        }

        [HttpGet("sendConfirmEmailToken")]
        public async Task<IActionResult> SendConfirmEmailTokenAsync(string? userId)
        {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(userId))
                return NotFound();

            var result = await _accountService.SendConfirmEmailTokenAsync(userId);

            return result ? Ok("Email sent") : BadRequest("Sent email error");
        }
    }
}
