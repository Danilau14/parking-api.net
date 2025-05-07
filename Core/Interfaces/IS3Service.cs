namespace ParkingApi.Core.Interfaces;

public interface IS3Service
{
    Task UploadFileAsync(string bucketName, string key, Stream fileStream);
    Task<Stream> DownloadFileAsync(string bucketName, string key);
    Task DeleteFileAsync(string bucketName, string key);
    Task<IEnumerable<string>> ListFilesAsync(string bucketName);
    Task<bool> BucketExistsAsync(string bucketName);
    Task CreateBucketIfNotExistsAsync(string bucketName);
    Task<IEnumerable<string>> ListBucketsAsync();
}
