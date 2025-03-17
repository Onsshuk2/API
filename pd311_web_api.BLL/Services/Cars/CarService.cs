using AutoMapper;
using Microsoft.EntityFrameworkCore;
using pd311_web_api.BLL.DTOs.Car;
using pd311_web_api.BLL.Services.Image;
using pd311_web_api.DAL.Entities;
using pd311_web_api.DAL.Repositories.Cars;
using pd311_web_api.DAL.Repositories.Manufactures;

namespace pd311_web_api.BLL.Services.Cars
{
    public class CarService : ICarService
    {
        private readonly ICarRepository _carRepository;
        private readonly IManufactureRepository _manufactureRepository;
        private readonly IImageService _imageService;
        private readonly IMapper _mapper;

        public CarService(ICarRepository carRepository, IMapper mapper, IManufactureRepository manufactureRepository, IImageService imageService)
        {
            _carRepository = carRepository;
            _mapper = mapper;
            _manufactureRepository = manufactureRepository;
            _imageService = imageService;
        }

        public async Task<ServiceResponse> CreateAsync(CreateCarDto dto)
        {
            var entity = _mapper.Map<Car>(dto);

            if (!string.IsNullOrEmpty(dto.Manufacture))
            {
                entity.Manufacture = await _manufactureRepository
                    .GetByNameAsync(dto.Manufacture);
            }

            if (dto.Images.Count() > 0)
            {
                string imagesPath = Path.Combine(Settings.CarsImagesPath, entity.Id.ToString()); // .ToString()
                _imageService.CreateImagesDirectory(imagesPath);
                var images = await _imageService.SaveCarImagesAsync(dto.Images, imagesPath);
                entity.Images = images;
            }


            var result = await _carRepository.CreateAsync(entity);

            if(!result)
            {
                return new ServiceResponse("Не вдалося зберегти автомобіль");
            }

            return new ServiceResponse($"Автомобіль '{entity.Brand} {entity.Model}' збережено", true);
        }

        public async Task<ServiceResponse> DeleteAsync(string id)
        {
            var entity = await _carRepository
                .GetAll()
                .Include(e => e.Images)
                .FirstOrDefaultAsync(e => e.Id == id);

            if (entity == null)
            {
                return new ServiceResponse("Автомобіль не знайдено");
            }

            // Видалення зображень автомобіля
            if (entity.Images != null && entity.Images.Any())
            {
                _imageService.DeleteCarImages(entity.Images);
            }

            var result = await _carRepository.DeleteAsync(entity);

            if (!result)
            {
                return new ServiceResponse("Не вдалося видалити автомобіль");
            }

            return new ServiceResponse($"Автомобіль '{entity.Brand} {entity.Model}' видалено", true);
        }




        public async Task<ServiceResponse> GetAllAsync()
        {
            var entities = await _carRepository
                .GetAll()
                .Include(e => e.Manufacture)
                .Include(e => e.Images)
                .ToListAsync();

            var dtos = _mapper.Map<List<CarDto>>(entities);

            return new ServiceResponse("Автомобілі отримано", true, dtos);
        }
        public async Task<ServiceResponse> GetByIdAsync(string id)
        {
            var entity = await _carRepository
                .GetAll()
                .Include(e => e.Manufacture)
                .Include(e => e.Images)
                .FirstOrDefaultAsync(e => e.Id == id);

            if (entity == null)
            {
                return new ServiceResponse("Автомобіль не знайдено");
            }

            var dto = _mapper.Map<CarDto>(entity);
            return new ServiceResponse("Автомобіль знайдений", true, dto);
        }

        public Task<ServiceResponse> UpdateAsync(string id, CreateCarDto dto)
        {
            throw new NotImplementedException();
        }
    }
}
