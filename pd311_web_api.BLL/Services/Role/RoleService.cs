using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using pd311_web_api.BLL.DTOs.Role;
using static pd311_web_api.DAL.Entities.IdentityEntities;

namespace pd311_web_api.BLL.Services.Role
{
    public class RoleService : IRoleService
    {
        private readonly RoleManager<AppRole> _roleManager;

        public RoleService(RoleManager<AppRole> roleManager)
        {
            _roleManager = roleManager;
        }

        public async Task<bool> CreateAsync(RoleDto dto)
        {
            var entity = new AppRole
            {
                Id = dto.Id ?? Guid.NewGuid().ToString(),
                Name = dto.Name
            };

            var result = await _roleManager.CreateAsync(entity);

            return result.Succeeded;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var entity = await _roleManager.FindByIdAsync(id);

            if (entity != null)
            {
                var result = await _roleManager.DeleteAsync(entity);
                return result.Succeeded;
            }

            return false;
        }

        public async Task<List<RoleDto>> GetAllAsync()
        {
            var entities = await _roleManager.Roles.ToListAsync();
            var dtos = entities.Select(r => new RoleDto
            {
                Id = r.Id,
                Name = r.Name
            });

            return dtos.ToList();
        }

        public async Task<RoleDto?> GetByIdAsync(string id)
        {
            var entity = await _roleManager.FindByIdAsync(id);

            if (entity == null)
                return null;

            var dto = new RoleDto
            {
                Id = entity.Id,
                Name = entity.Name
            };

            return dto;
        }

        public async Task<bool> UpdateAsync(RoleDto dto)
        {
            var entity = new AppRole
            {
                Id = dto.Id ?? Guid.NewGuid().ToString(),
                Name = dto.Name
            };

            var result = await _roleManager.UpdateAsync(entity);

            return result.Succeeded;
        }
    }
}
