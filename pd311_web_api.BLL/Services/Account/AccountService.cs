using AutoMapper;
using Microsoft.AspNetCore.Identity;
using pd311_web_api.BLL.DTOs.Account;
using pd311_web_api.BLL.Services.Email;
using System.Text;
using static pd311_web_api.DAL.Entities.IdentityEntities;

namespace pd311_web_api.BLL.Services.Account
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<AppRole> _roleManager;
        private readonly IEmailService _emailService;
        private readonly IMapper _mapper;

        public AccountService(UserManager<AppUser> userManager, IEmailService emailService, IMapper mapper, RoleManager<AppRole> roleManager)
        {
            _userManager = userManager;
            _emailService = emailService;
            _mapper = mapper;
            _roleManager = roleManager;
        }

        public async Task<bool> ConfirmEmailAsync(string id, string base64)
        {
            var user = await _userManager.FindByIdAsync(id);

            if(user != null)
            {
                var bytes = Convert.FromBase64String(base64);
                var token = Encoding.UTF8.GetString(bytes);
                var result = await _userManager.ConfirmEmailAsync(user, token);
                return result.Succeeded;
            }

            return false;
        }

        public async Task<ServiceResponse> LoginAsync(LoginDto dto)
        {
            var user = await _userManager.FindByNameAsync(dto.UserName ?? "");

            if (user == null)
                return new ServiceResponse($"Користувача з іменем '{dto.UserName}' не знайдено");

            var result = await _userManager.CheckPasswordAsync(user, dto.Password ?? "");

            if (!result)
                return new ServiceResponse($"Пароль вказано невірно");

            return new ServiceResponse("Успішний вхід", true, user);
        }

        public async Task<AppUser?> RegisterAsync(RegisterDto dto)
        {
            if (await _userManager.FindByEmailAsync(dto.Email) != null)
                return null;

            if (await _userManager.FindByNameAsync(dto.UserName) != null)
                return null;

            var user = _mapper.Map<AppUser>(dto);

            var result = await _userManager.CreateAsync(user, dto.Password);

            if(result.Succeeded && await _roleManager.RoleExistsAsync("user"))
            {
                result = await _userManager.AddToRoleAsync(user, "user");
            }

            if (!result.Succeeded)
                return null;

            await SendConfirmEmailTokenAsync(user.Id);

            return user;
        }

        public async Task<bool> SendConfirmEmailTokenAsync(string userId)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId);

                if (user == null)
                {
                    return false;
                }

                // Sent mail
                var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                var bytes = Encoding.UTF8.GetBytes(token);
                var base64 = Convert.ToBase64String(bytes);

                var body = $"<a href='https://localhost:7223/api/account/confirmEmail?id={user.Id}&t={base64}'>Підтвердити пошту</a>";

                await _emailService.SendMailAsync(user.Email!, "Email confirm", body, true);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
