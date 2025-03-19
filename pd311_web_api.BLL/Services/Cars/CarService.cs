using AutoMapper;
using Microsoft.EntityFrameworkCore;
using pd311_web_api.BLL.DTOs.Car;
using pd311_web_api.DAL.Entities;
using pd311_web_api.DAL.Repositories.Cars;
using pd311_web_api.DAL.Repositories.Manufactures;

namespace pd311_web_api.BLL.Services.Cars
{
    public class CarService : ICarService
    {
        private readonly ICarRepository _carRepository;
        private readonly IMapper _mapper;
        private readonly DbContext _context;

        public CarService(ICarRepository carRepository, IMapper mapper)
        {
            _carRepository = carRepository;
            _mapper = mapper;
        }
       

        public async Task AddAsync(Car car)
        {
            await _context.Set<Car>().AddAsync(car);
            await _context.SaveChangesAsync();
        }

        private async Task<ServiceResponse> GetPaginatedCars(IQueryable<Car> query, int page, int pageSize)
        {
            var totalRecords = await query.CountAsync();
            var cars = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
            var dtos = _mapper.Map<List<CarDto>>(cars);

            return new ServiceResponse("Автомобілі отримано", true, new { TotalRecords = totalRecords, Cars = dtos });
        }

        public async Task<ServiceResponse> GetByManufacturerAsync(string manufacturer, int page, int pageSize)
        {
            var query = _carRepository.GetAll().Where(c => c.Manufacture.Name == manufacturer);
            return await GetPaginatedCars(query, page, pageSize);
        }

        public async Task<ServiceResponse> GetByYearAsync(int year, int page, int pageSize)
        {
            var query = _carRepository.GetAll().Where(c => c.Year == year);
            return await GetPaginatedCars(query, page, pageSize);
        }

        public async Task<ServiceResponse> GetByTransmissionAsync(string transmission, int page, int pageSize)
        {
            var query = _carRepository.GetAll().Where(c => c.Transmission == transmission);
            return await GetPaginatedCars(query, page, pageSize);
        }

        public async Task<ServiceResponse> GetByColorAsync(string color, int page, int pageSize)
        {
            var query = _carRepository.GetAll().Where(c => c.Color == color);
            return await GetPaginatedCars(query, page, pageSize);
        }

        public async Task<ServiceResponse> SearchByModelAsync(string searchTerm, int page, int pageSize)
        {
            var query = _carRepository.GetAll().Where(c => c.Model.Contains(searchTerm));
            return await GetPaginatedCars(query, page, pageSize);
        }
        public async Task<ServiceResponse> CreateAsync(CreateCarDto carDto)
        {
            var car = _mapper.Map<Car>(carDto);
            await _carRepository.AddAsync(car);
            return new ServiceResponse("Автомобіль створено", true);
        }

        public async Task<ServiceResponse> DeleteAsync(string id)
        {
            var car = await _carRepository.GetByIdAsync(id);
            if (car == null)
            {
                return new ServiceResponse("Автомобіль не знайдений", false);
            }
            await _carRepository.DeleteAsync(car);
            return new ServiceResponse("Автомобіль видалено", true);
        }

        public async Task<ServiceResponse> GetAllAsync()
        {
            var cars = await _carRepository.GetAll().ToListAsync();
            var dtos = _mapper.Map<List<CarDto>>(cars);
            return new ServiceResponse("Автомобілі отримано", true, dtos);
        }

        public async Task<ServiceResponse> GetByIdAsync(string id)
        {
            var car = await _carRepository.GetByIdAsync(id);
            if (car == null)
            {
                return new ServiceResponse("Автомобіль не знайдений", false);
            }
            var dto = _mapper.Map<CarDto>(car);
            return new ServiceResponse("Автомобіль отримано", true, dto);
        }

        public async Task<ServiceResponse> UpdateAsync(string id, CreateCarDto carDto)
        {
            var car = await _carRepository.GetByIdAsync(id);
            if (car == null)
            {
                return new ServiceResponse("Автомобіль не знайдений", false);
            }
            _mapper.Map(carDto, car);
            await _carRepository.UpdateAsync(car);
            return new ServiceResponse("Автомобіль оновлено", true);
        }

        
    }
}
