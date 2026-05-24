namespace GymSystem.Web.Services
{
    public interface IPhotoService
    {
        Task<string> SaveAsync(IFormFile file, string folder);
        void Delete(string? relativePath);
    }
}
