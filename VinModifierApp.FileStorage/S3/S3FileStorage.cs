using System;
using Amazon;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Transfer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace VinModifierApp.FileStorage.S3;

public class S3FileStorage : IFileStorage
{
    public IConfiguration Config { get; }
    public ILogger<S3FileStorage> Logger { get; }

    public S3FileStorage(IConfiguration config, ILogger<S3FileStorage> logger)
    {
        Config = config;
        Logger = logger;
    }

    public async Task<bool> SaveFile(IFormFile file)
    {
        try
        {
            var credentials = new BasicAWSCredentials(Config["S3:AccessKeyId"], Config["S3:Secret"]);

            using (var s3Client = new AmazonS3Client(credentials, RegionEndpoint.USEast2))
            {
                using (var memoryStream = new MemoryStream())
                {
                    file.CopyTo(memoryStream);
                    var uploadRequest = new TransferUtilityUploadRequest
                    {
                        InputStream = memoryStream,
                        Key = $"{Guid.NewGuid()}_{file.FileName}",
                        BucketName = Config["S3:Bucket"],
                        CannedACL = S3CannedACL.NoACL,
                        ContentType = file.ContentType
                    };
                    var fileTranserUtitliy = new TransferUtility(s3Client);
                    await fileTranserUtitliy.UploadAsync(uploadRequest);
                }
            }
            return true;
        }
        catch (Exception ex)
        {
            Logger.LogError(new EventId(), ex, ex.Message);
            return false;
        }

    }
}
