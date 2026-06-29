namespace ShoeStore.API.Services.Interfaces;

public interface IImageService
{
    Task<string> SaveImageAsync(string base64Image, string fileName);
    void DeleteImage(string imagePath);
}