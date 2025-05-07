using Amazon.S3.Model;
using Amazon.S3;

namespace ParkingApi.Application.Services;

public class S3Service : IS3Service
{
    private readonly IAmazonS3 _s3Client;

    public S3Service(IAmazonS3 s3Client)
    {
        _s3Client = s3Client;
    }

    public async Task UploadFileAsync(string bucketName, string key, Stream fileStream)
    {
        var request = new PutObjectRequest
        {
            BucketName = bucketName,
            Key = key,
            InputStream = fileStream
        };
        await _s3Client.PutObjectAsync(request);
    }

    public async Task<Stream> DownloadFileAsync(string bucketName, string key)
    {
        var response = await _s3Client.GetObjectAsync(bucketName, key);
        return response.ResponseStream;
    }

    public async Task DeleteFileAsync(string bucketName, string key)
    {
        await _s3Client.DeleteObjectAsync(bucketName, key);
    }

    public async Task<IEnumerable<string>> ListFilesAsync(string bucketName)
    {
        var response = await _s3Client.ListObjectsV2Async(new ListObjectsV2Request
        {
            BucketName = bucketName
        });

        return response.S3Objects.Select(o => o.Key);
    }

    public async Task<bool> BucketExistsAsync(string bucketName)
    {
        var response = await _s3Client.ListBucketsAsync();
        return response.Buckets.Any(b => b.BucketName == bucketName);
    }

    public async Task CreateBucketIfNotExistsAsync(string bucketName)
    {
        if (!await BucketExistsAsync(bucketName))
        {
            await _s3Client.PutBucketAsync(new PutBucketRequest
            {
                BucketName = bucketName
            });
        }
    }

    public async Task<IEnumerable<string>> ListBucketsAsync()
    {
        var response = await _s3Client.ListBucketsAsync();
        return response.Buckets.Select(b => b.BucketName);
    }
}
