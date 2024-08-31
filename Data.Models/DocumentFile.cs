namespace Data.Models;

public class DocumentFile
{
    public string OriginalName { get; set; }
    public string Extension { get; set; }
    public string Path { get; set; }
    public string FileName { get; set; }

    public static DocumentFile Empty()
    {
        return new DocumentFile()
        {
            OriginalName = String.Empty,
            Extension = String.Empty,
            Path = String.Empty,
            FileName = String.Empty
        };
    }
}