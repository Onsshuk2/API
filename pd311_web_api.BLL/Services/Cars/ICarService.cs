using pd311_web_api.BLL.DTOs.Car;

namespace pd311_web_api.BLL.Services.Cars
{
    public interface ICarService
    {
        Task<ServiceResponse> CreateAsync(CreateCarDto dto);
        Task<ServiceResponse> GetAllAsync();
        Task<ServiceResponse> GetByIdAsync(string id); 
        Task<ServiceResponse> UpdateAsync(string id, CreateCarDto dto);
        Task<ServiceResponse> DeleteAsync(string id);
    }
}
