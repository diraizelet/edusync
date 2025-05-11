using Azure.Storage.Blobs;
using Microsoft.Extensions.Options;

namespace edusync.Models
{
    public interface IBlobStorageService
    {
        Task<string> UploadFileAsync(IFormFile file, string containerKey);
    }

    public class BlobStorageService : IBlobStorageService
    {
        private readonly BlobStorageSettings _settings;

        public BlobStorageService(IOptions<BlobStorageSettings> options)
        {
            _settings = options.Value;
        }

        public async Task<string> UploadFileAsync(IFormFile file, string containerKey)
        {
            if (!_settings.Containers.TryGetValue(containerKey, out var containerName))
                throw new ArgumentException("Invalid container key");

            var blobServiceClient = new BlobServiceClient(_settings.ConnectionString);
            var containerClient = blobServiceClient.GetBlobContainerClient(containerName);
            await containerClient.CreateIfNotExistsAsync();

            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
            var blobClient = containerClient.GetBlobClient(fileName);

            using (var stream = file.OpenReadStream())
            {
                await blobClient.UploadAsync(stream, overwrite: true);
            }

            return blobClient.Uri.ToString();
        }
    }
}
