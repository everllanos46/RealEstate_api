using Google.Cloud.Storage.V1;
using RealEstate.Domain.Interfaces;
using Microsoft.Extensions.Configuration;
using Google.Apis.Auth.OAuth2;

namespace RealEstate.Infrastructure.Services;

public class FirebaseStorageRepository : IFileStorageRepository
{
    private readonly string _bucketName;
    private readonly GoogleCredential _credential;
    private readonly string _credentialsPath;

    public FirebaseStorageRepository(IConfiguration config)
    {
        _bucketName = config["Firebase:Bucket"] ?? throw new ArgumentNullException("Firebase:Bucket");;
        _credentialsPath = config["Firebase:CredentialsPath"] ?? throw new ArgumentNullException("Firebase:CredentialsPath");;

        if (string.IsNullOrEmpty(_credentialsPath))
            throw new InvalidOperationException("Firebase credentials path is not configured.");

        _credential = GoogleCredential.FromFile(_credentialsPath);
    }

    public async Task<string> UploadAsync(string path, Stream content, string contentType)
    {
        var storage = await StorageClient.CreateAsync(_credential);

        await storage.UploadObjectAsync(_bucketName, path, contentType, content);
        return GenerateSignedUrl(path, TimeSpan.FromHours(1));
    }

    public async Task DeleteAsync(string path)
    {
        var storage = await StorageClient.CreateAsync(_credential);
        await storage.DeleteObjectAsync(_bucketName, path);
    }

    private string GenerateSignedUrl(string path, TimeSpan duration)
    {
        var signer = UrlSigner.FromCredentialFile(_credentialsPath);
        return signer.Sign(
            _bucketName,
            path,
            duration,
            HttpMethod.Get
        );
    }
}
