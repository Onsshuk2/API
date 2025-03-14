﻿using Microsoft.AspNetCore.Identity;
using pd311_web_api.BLL.DTOs.Account;
using pd311_web_api.BLL.Services.Email;
using System.Text;
using static pd311_web_api.DAL.Entities.IdentityEntities;

namespace pd311_web_api.BLL.Services.Account
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IEmailService _emailService;

        public AccountService(UserManager<AppUser> userManager, IEmailService emailService)
        {
            _userManager = userManager;
            _emailService = emailService;
        }

        public async Task<bool> ConfirmEmailAsync(string id, string token)
        {
            // Шукаємо користувача за його ID
            var user = await _userManager.FindByIdAsync(id);

            // Якщо користувач не знайдений, повертаємо false
            if (user == null) return false;

            // Підтверджуємо email за допомогою токену
            var result = await _userManager.ConfirmEmailAsync(user, token);

            // Повертаємо результат підтвердження 
            return result.Succeeded;
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

        public async Task<AppUser?> LoginAsync(LoginDto dto)
        {
            var user = await _userManager.FindByNameAsync(dto.UserName ?? "");
            if (user == null) return null;

            var result = await _userManager.CheckPasswordAsync(user, dto.Password ?? "");
            return result ? user : null;
        }

        public async Task<IdentityResult> RegisterAsync(RegisterDto dto)
        {
            if (await _userManager.FindByEmailAsync(dto.Email) != null)
                return IdentityResult.Failed(new IdentityError { Description = "Email is already taken." });

            if (await _userManager.FindByNameAsync(dto.UserName) != null)
                return IdentityResult.Failed(new IdentityError { Description = "Username is already taken." });

            var user = new AppUser
            {
                Email = dto.Email,
                UserName = dto.UserName
            };

            var result = await _userManager.CreateAsync(user, dto.Password);

            if (!result.Succeeded)
                return result;

            // Надсилання підтвердження пошти
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var body = $"<a href='https://localhost:7223/api/account/confirmEmail?id={user.Id}&t={token}'>Підтвердити пошту</a>";

            await _emailService.SendMailAsync(user.Email, "Email confirm", body, true);

            return result;
        }
    }
}
