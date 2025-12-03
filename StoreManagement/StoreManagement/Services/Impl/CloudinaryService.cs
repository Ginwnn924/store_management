using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using StoreManagement.Settings;
using System.Net;

namespace StoreManagement.Services.Impl
{
    public class CloudinaryService : ICloudStorageService
    {
        private readonly Cloudinary _cloudinary;
        private readonly CloudinarySettings _settings;

        public CloudinaryService(IOptions<CloudinarySettings> options)
        {
            _settings = options?.Value ?? throw new ArgumentNullException(nameof(options));

            if (string.IsNullOrWhiteSpace(_settings.CloudName) ||
                string.IsNullOrWhiteSpace(_settings.ApiKey) ||
                string.IsNullOrWhiteSpace(_settings.ApiSecret))
            {
                throw new InvalidOperationException("Cloudinary settings are not configured properly.");
            }

            var account = new Account(_settings.CloudName, _settings.ApiKey, _settings.ApiSecret);
            _cloudinary = new Cloudinary(account);
        }

        public async Task<string> UploadImageAsync(IFormFile file, CancellationToken cancellationToken = default)
        {
            if (file is null || file.Length == 0)
            {
                throw new ArgumentException("Image file must not be empty.", nameof(file));
            }

            await using var stream = file.OpenReadStream();
            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(file.FileName, stream),
                Folder = _settings.ProductFolder,
                UseFilename = true,
                UniqueFilename = true,
                Overwrite = false
            };

            var uploadResult = await _cloudinary.UploadAsync(uploadParams, cancellationToken);

            if (uploadResult is null ||
                uploadResult.StatusCode >= HttpStatusCode.BadRequest)
            {
                throw new InvalidOperationException("Không thể tải ảnh lên Cloudinary.");
            }

            return uploadResult.SecureUrl?.AbsoluteUri
                ?? uploadResult.Url?.AbsoluteUri
                ?? throw new InvalidOperationException("Cloudinary không trả về URL hợp lệ.");
        }
    }
}

