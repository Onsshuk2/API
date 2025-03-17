using Microsoft.AspNetCore.Mvc;
using pd311_web_api.BLL.DTOs.Car;
using pd311_web_api.BLL.Services.Cars;

namespace pd311_web_api.Controllers
{
    [ApiController]
    [Route("api/car")]
    public class CarController : BaseController
    {
        private readonly ICarService _carService;

        public CarController(ICarService carService)
        {
            _carService = carService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync(CreateCarDto dto)
        {
            var response = await _carService.CreateAsync(dto);
            return CreateActionResult(response);
        }

        [HttpGet("list")]
        public async Task<IActionResult> GetAllAsync()
        {
            var response = await _carService.GetAllAsync();
            return CreateActionResult(response);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(string id)
        {
            var response = await _carService.GetByIdAsync(id);
            return CreateActionResult(response);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(string id, CreateCarDto dto)
        {
            var response = await _carService.UpdateAsync(id, dto);
            return CreateActionResult(response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(string id)
        {
            var response = await _carService.DeleteAsync(id);
            return CreateActionResult(response);
        }

    }
}
