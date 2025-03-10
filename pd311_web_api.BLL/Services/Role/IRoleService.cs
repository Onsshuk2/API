using pd311_web_api.BLL.DTOs.Role;

namespace pd311_web_api.BLL.Services.Role
{
    public interface IRoleService
    {
        public Task<bool> CreateAsync(RoleDto dto);
        public Task<bool> UpdateAsync(RoleDto dto);
        public Task<bool> DeleteAsync(string id);
        public Task<RoleDto?> GetByIdAsync(string id);
        public Task<List<RoleDto>> GetAllAsync();
    }
}
