using pd311_web_api.DAL.Entities;

namespace pd311_web_api.DAL.Repositories.Cars
{
    public interface ICarRepository
        : IGenericRepository<Car, string>
    {
        Task AddAsync(Car car);
    }
}
