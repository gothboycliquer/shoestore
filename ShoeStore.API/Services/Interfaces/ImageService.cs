using ShoeStore.API.Services.Interfaces;

namespace ShoeStore.API.Services;

public class ImageService : IImageService
{
    private readonly IWebHostEnvironment _environment;

    public ImageService(IWebHostEnvironment environment)
    {
        _environment = environment;
    }

    public async Task<string> SaveImageAsync(string base64Image, string fileName)
    {
        var imagesFolder = Path.Combine(_environment.ContentRootPath, "Images");

        if (!Directory.Exists(imagesFolder))
            Directory.CreateDirectory(imagesFolder);

        var imageBytes = Convert.FromBase64String(base64Image);
        var uniqueFileName = $"{Guid.NewGuid()}_{fileName}";
        var filePath = Path.Combine(imagesFolder, uniqueFileName);

        await File.WriteAllBytesAsync(filePath, imageBytes);

        return filePath;
    }

    public void DeleteImage(string imagePath)
    {
        if (File.Exists(imagePath))
            File.Delete(imagePath);
    }
}