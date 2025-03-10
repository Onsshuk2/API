using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace pd311_web_api.DAL.Entities
{
    public class Car
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();
        [Required]
        [MaxLength(100)]
        public required string Model { get; set; }
        [Required]
        [MaxLength(100)]
        public required string Brand { get; set; }
        public int Year { get; set; }
    }
}
