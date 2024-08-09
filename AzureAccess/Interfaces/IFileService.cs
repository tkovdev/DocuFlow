namespace AzureAccess.Interfaces;

public interface IFileService
{
    public Task<Stream> GetFile(string fileName, string path);
    public Task<string> SaveFile(Stream file, string path);
    public Task DeleteFile(string fileName);
}