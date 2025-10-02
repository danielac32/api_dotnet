public class FileUploadRequest
{
    public FileData File { get; set; }
}

public class FileUploadRequestList
{
    public List<FileData> Files { get; set; } = new List<FileData>();
}

public class FileData
{
    public string Filename { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty; // Base64
}


public class UploadFileDto
{
    public string Filename { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty; // Base64
}