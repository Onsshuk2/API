using Microsoft.AspNetCore.Http;
using pd311_web_api.DAL.Entities;


namespace pd311_web_api.BLL.Services.Image
{
    public class ImageService : IImageService
    {
        private readonly string ImagesPath;

      
        public ImageService()
        {
            ImagesPath = Path.Combine(Settings.RootPath, "wwwroot", Settings.RootImagesPath);
        }

       
        public void CreateImagesDirectory(string path)
        {
            string workPath = Path.Combine(ImagesPath, path);
            if (!Directory.Exists(workPath))
            {
                Directory.CreateDirectory(workPath);
            }
        }

        public void DeleteCarImages(List<CarImage> carImages)
        {
            try
            {
                foreach (var carImage in carImages)
                {
                    var imagePath = Path.Combine(ImagesPath, carImage.Path, carImage.Name);

                   
                    if (File.Exists(imagePath))
                    {
                        File.Delete(imagePath);
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

        public void DeleteCarImages(ICollection<CarImage> images)
        {
            throw new NotImplementedException();
        }

        public void DeleteImage(string imagePath)
        {
            try
            {
                if (File.Exists(imagePath))
                {
                    File.Delete(imagePath);
                }
            }
            catch (Exception ex)
            {
                // Log exception if needed
                throw new InvalidOperationException("Error while deleting image.", ex);
            }
        }

        public async Task<List<CarImage>> SaveCarImagesAsync(IEnumerable<IFormFile> images, string directoryPath)
        {
            var carImages = new List<CarImage>();

            foreach (var img in images)
            {
                var imageName = await SaveImageAsync(img, directoryPath);
                if (imageName != null)
                {
                    var carImage = new CarImage
                    {
                        Name = imageName,
                        Path = directoryPath
                    };
                    carImages.Add(carImage);
                }
            }

            return carImages;
        }

        public async Task<string?> SaveImageAsync(IFormFile image, string directoryPath)
        {
            try
            {
                var types = image.ContentType.Split("/");

                if (types[0] != "image" || !new[] { "jpeg", "png", "gif", "bmp", "tiff" }.Contains(types[1].ToLower()))
                {
                    // If not a valid image, return null
                    return null;
                }

                var imageName = $"{Guid.NewGuid()}.{types[1]}";
                var imagePath = Path.Combine(ImagesPath, directoryPath, imageName);

                using (var stream = File.Create(imagePath))
                {
                    await image.CopyToAsync(stream);
                }

                return imageName;
            }
            catch (Exception ex)
            {
                
                return null;
            }
        }
    }
}
