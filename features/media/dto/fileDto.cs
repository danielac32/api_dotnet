public class FileUploadRequest
{
    public FileData File { get; set; }
}

public class FileData
{
    public string Filename { get; set; }
    public string Content { get; set; } // Base64
}


public class UploadFileDto
{
    public string Filename { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty; // Base64
}