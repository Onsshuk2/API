using pd311_web_api.BLL.DTOs.User;

namespace pd311_web_api.BLL.Services.User
{
    public interface IUserService
    {
        Task<List<UserDto>> GetAllAsync();
    }
}
