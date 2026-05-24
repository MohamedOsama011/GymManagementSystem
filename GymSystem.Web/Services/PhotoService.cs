namespace GymSystem.Web.Services
{
    public class PhotoService : IPhotoService
    {
        private readonly IWebHostEnvironment _env;

        private static readonly HashSet<string> _allowedExtensions =
            new(StringComparer.OrdinalIgnoreCase) { ".jpg", ".jpeg", ".png", ".webp" };

        private const long MaxFileSizeByte = 2 * 1024 * 1024;

        public PhotoService(IWebHostEnvironment env)
        {
            _env = env;
        }
        public async Task<string> SaveAsync(IFormFile file, string folder)
        {
            ValidateFile(file);

            var uploadFolder = Path.Combine(_env.WebRootPath, folder);
            Directory.CreateDirectory(uploadFolder);

            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
            var fileName = $"{Guid.NewGuid()}{extension}";
            var fullPath = Path.Combine(uploadFolder, fileName);

            await using var stream = new FileStream(fullPath, FileMode.Create);
            await file.CopyToAsync(stream);

            return Path.Combine(folder, fileName).Replace("\\", "/");
        }

        public void Delete(string? relativePath)
        {
            var fullPath = Path.Combine(_env.WebRootPath, relativePath);
            if (File.Exists(fullPath))
                File.Delete(fullPath);
        }

        private static void ValidateFile(IFormFile file)
        {
            if (file.Length > MaxFileSizeByte)
                throw new InvalidOperationException("Photo must be under 2 MB.");

            var ext = Path.GetExtension(file.FileName);
            if (!_allowedExtensions.Contains(ext))
                throw new InvalidOperationException("Only .jpg, .jpeg, .png, and .webp files are allowed.");
        }
    }
}
