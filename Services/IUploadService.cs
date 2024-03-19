namespace RestaurantApp.Services
{
    public interface IUploadService
    {
        Task<string> Upload(IFormFile file);
    }
}
