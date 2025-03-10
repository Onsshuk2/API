using Microsoft.AspNetCore.Mvc;
using pd311_web_api.DAL;
using pd311_web_api.DAL.Entities;

namespace pd311_web_api.Controllers
{
    [ApiController]
    [Route("api/car")]
    public class CarController : ControllerBase
    {
        private AppDbContext _context;

        public CarController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetCars()
        {
            var entities = _context.Cars.ToList();
            return Ok(entities);
        }

        [HttpPost]
        public IActionResult Create([FromBody] Car entity)
        {
            _context.Cars.Add(entity);
            _context.SaveChanges();
            return Ok("Автомобіль створено");
        }
    }
}
