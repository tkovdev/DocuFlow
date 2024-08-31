namespace Data.Access.Abstractions.Interfaces;

public interface IFileService
{
    public Task<Stream> GetFile(string fileName);
    public Task<string> SaveFile(Stream file, string path);
    public Task DeleteFile(string fileName);
}