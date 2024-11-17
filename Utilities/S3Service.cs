using Amazon.S3;
using Amazon.S3.Model;

namespace BookManagement.Utilities
{
    public class S3Service
    {
        private readonly IAmazonS3 _s3Client;
        private const string BucketName = "your-book-covers-bucket";

        public S3Service()
        {
            _s3Client = new AmazonS3Client();
        }

        public async Task<string> UploadBookCoverAsync(Stream fileStream, string isbn, string fileName)
        {
            var putRequest = new PutObjectRequest
            {
                InputStream = fileStream,
                BucketName = BucketName,
                Key = $"covers/{isbn}.jpg",  // Save cover with ISBN as filename
                ContentType = "image/jpeg"
            };

            var response = await _s3Client.PutObjectAsync(putRequest);

            return $"https://{BucketName}.s3.amazonaws.com/covers/{isbn}/{fileName}";
            //Console.WriteLine($"Uploaded cover for ISBN: {isbn}");
        }
    }
}
