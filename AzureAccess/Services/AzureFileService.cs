using Azure.Storage;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using AzureAccess.Interfaces;
using Microsoft.Extensions.Configuration;

namespace AzureAccess.Services;

public class AzureFileService : IFileService
{
    private BlobServiceClient _blobServiceClient;
    private IConfiguration _configuration;

    public AzureFileService(IConfiguration configuration)
    {
        _configuration = configuration;
        var saName = _configuration.GetSection("Azure:Storage").GetValue<string>("Name");
        var saKey = _configuration.GetSection("Azure:Storage").GetValue<string>("Key");
        _blobServiceClient = GetBlobServiceClient(saName, saKey);
    }

    private BlobServiceClient GetBlobServiceClient(
        string accountName, string accountKey)
    {
        StorageSharedKeyCredential sharedKeyCredential =
            new StorageSharedKeyCredential(accountName, accountKey);

        string blobUri = "https://" + accountName + ".blob.core.windows.net";

        return new BlobServiceClient
            (new Uri(blobUri), sharedKeyCredential);
    }
    
    public async Task<Stream> GetFile(string fileName)
    {
        var container = _blobServiceClient.GetBlobContainerClient("docuflow");
        var blob = container.GetBlobClient(fileName);

        var stream = new MemoryStream();
        var result = await blob.DownloadToAsync(stream);
        if (result.IsError) throw new Exception("File download from Azure failed!");
        return stream;
    }

    public async Task<string> SaveFile(Stream file, string path)
    {
        var container = _blobServiceClient.GetBlobContainerClient("docuflow");
        var blob = container.GetBlobClient(path);

        var result = await blob.UploadAsync(file, true);
        if (result.GetRawResponse().IsError) throw new Exception("File upload to Azure failed!");
        return blob.Uri.AbsoluteUri;
    }

    public async Task DeleteFile(string fileName)
    {
        var container = _blobServiceClient.GetBlobContainerClient("docuflow");
        var result = await container.DeleteBlobIfExistsAsync(fileName, DeleteSnapshotsOption.IncludeSnapshots);

        if (result.Value) return;
        
        throw new Exception("File deletion in Azure failed or file not found!");
    }
}