using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;
using Azure.Storage.Blobs;
using System;
using System.IO;
using System.Threading.Tasks;

namespace edusync.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileUploadController : ControllerBase
    {
        private readonly string[] permittedExtensions = { ".pdf", ".doc", ".docx" };
        private readonly long fileSizeLimit = 10 * 1024 * 1024; // 10MB
        private readonly BlobServiceClient _blobServiceClient;
        private readonly string _courseMaterialContainer;
        private readonly string _courseAssessmentContainer;

        public FileUploadController(IConfiguration configuration)
        {
            var connectionString = configuration["AzureBlobStorage:ConnectionString"];
            _courseMaterialContainer = configuration["AzureBlobStorage:Containers:CourseMaterial"];
            _courseAssessmentContainer = configuration["AzureBlobStorage:Containers:CourseAssessment"];
            _blobServiceClient = new BlobServiceClient(connectionString);
        }

        [HttpPost("course-material")]
        public async Task<IActionResult> UploadCourseMaterial(IFormFile file)
        {
            return await UploadFileToContainer(file, _courseMaterialContainer);
        }

        [HttpPost("course-assessment")]
        public async Task<IActionResult> UploadCourseAssessment(IFormFile file)
        {
            return await UploadFileToContainer(file, _courseAssessmentContainer);
        }

        private async Task<IActionResult> UploadFileToContainer(IFormFile file, string containerName)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded.");

            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (string.IsNullOrEmpty(extension) || !Array.Exists(permittedExtensions, e => e == extension))
                return BadRequest("Invalid file type. Only PDF and Word documents are allowed.");

            if (file.Length > fileSizeLimit)
                return BadRequest("File size exceeds the 10MB limit.");

            try
            {
                var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
                await containerClient.CreateIfNotExistsAsync();

                var fileName = $"{Guid.NewGuid()}{extension}";
                var blobClient = containerClient.GetBlobClient(fileName);

                using (var stream = file.OpenReadStream())
                {
                    await blobClient.UploadAsync(stream, overwrite: true);
                }

                var blobUrl = blobClient.Uri.ToString();
                return Ok(new { fileName, url = blobUrl });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
