using Microsoft.AspNetCore.Identity;
using pd311_web_api.BLL.DTOs.Account;
using static pd311_web_api.DAL.Entities.IdentityEntities;

namespace pd311_web_api.BLL.Services.Account
{
    public interface IAccountService
    {
        Task<AppUser?> LoginAsync(LoginDto dto);
        Task<IdentityResult> RegisterAsync(RegisterDto dto);
        Task<bool> ConfirmEmailAsync(string id, string token);
    }


}
