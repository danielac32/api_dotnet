

public class FileUploadDto
    {
        public string FileName { get; set; } = string.Empty;
        public string FileData { get; set; } = string.Empty;
        public long FileSize { get; set; }
        public string UploadDate { get; set; } = string.Empty;
    }