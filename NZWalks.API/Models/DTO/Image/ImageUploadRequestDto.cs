using Microsoft.Build.Framework;

namespace NZWalks.API.Models.DTO.Image;

public class ImageUploadRequestDto
{
    [Required] public IFormFile File { get; set; }

    [Required] public string FileName { get; set; }

    public string? FileDescription { get; set; }
}