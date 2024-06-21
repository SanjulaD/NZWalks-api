using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO.Image;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ImagesController : ControllerBase
{
    private readonly IImageRepository _imageRepository;

    public ImagesController(IImageRepository imageRepository)
    {
        _imageRepository = imageRepository;
    }

    [HttpPost]
    [Route("Upload")]
    public async Task<IActionResult> Upload([FromForm] ImageUploadRequestDto request)
    {
        ValidateFileUpload(request);

        if (ModelState.IsValid)
        {
            var imageModelDomain = new Image
            {
                File = request.File,
                FileExtenstion = Path.GetExtension(request.File.FileName),
                FileSizeInBytes = request.File.Length,
                FileName = request.FileName,
                FileDescription = request.FileName
            };

            await _imageRepository.Upload(imageModelDomain);

            return Ok(imageModelDomain);
        }

        return BadRequest(ModelState);
    }

    private void ValidateFileUpload(ImageUploadRequestDto request)
    {
        var allowedExtensions = new string[] { ".jpg", "jpeg", ".png" };
        if (!allowedExtensions.Contains(Path.GetExtension(request.File.FileName)))
            ModelState.AddModelError("file", "Unsupported file extension");

        if (request.File.Length > 10485760)
            ModelState.AddModelError("file", "File size more than 10mb, please upload smaller size file");
    }
}