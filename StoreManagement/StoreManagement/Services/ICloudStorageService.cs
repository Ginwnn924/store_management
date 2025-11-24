using Microsoft.AspNetCore.Http;

namespace StoreManagement.Services
{
    public interface ICloudStorageService
    {
        Task<string> UploadImageAsync(IFormFile file, CancellationToken cancellationToken = default);
    }
}

