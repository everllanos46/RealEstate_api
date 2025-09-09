namespace RealEstate.Application.DTOs;

public class UploadFileDto
{
    public string FileName { get; set; } = string.Empty;
    public Stream Content { get; set; } = Stream.Null;
    public string ContentType { get; set; } = string.Empty;
}
