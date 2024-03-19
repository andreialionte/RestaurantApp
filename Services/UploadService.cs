using Amazon.S3;

namespace RestaurantApp.Services
{
    public class UploadService : IUploadService
    {
        private readonly IAmazonS3 _s3;

        public UploadService(IAmazonS3 s3)
        {
            _s3 = s3;
        }

        public async Task<string> Upload(IFormFile file)
        {
            string bucketName = "restaurantappbucket";
            string fileKey = file.FileName;

            using var memoryStream = new MemoryStream();
            await file.CopyToAsync(memoryStream);

            var putRequest = new Amazon.S3.Model.PutObjectRequest
            {
                BucketName = bucketName,
                Key = "Photos/" + fileKey,
                InputStream = memoryStream
            };

            await _s3.PutObjectAsync(putRequest);

            return $"https://{bucketName}.s3.amazonaws.com/Photos/{fileKey}";
        }
    }
}
