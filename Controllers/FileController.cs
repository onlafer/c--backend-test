using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;

public class FileController : Controller
{
    private readonly IContentTypeProvider _contentTypeProvider;

    public FileController(IContentTypeProvider contentTypeProvider)
    {
        _contentTypeProvider = contentTypeProvider;
    }

    [HttpGet("files/{category}/{filename}")]
    public IActionResult GetFile(string category, string filename)
    {
        var filePath = Path.Combine("StaticFiles", category, filename);

        if (!System.IO.File.Exists(filePath))
            return NotFound();

        if (!_contentTypeProvider.TryGetContentType(filename, out var contentType))
            contentType = "application/octet-stream";

        return PhysicalFile(Path.GetFullPath(filePath), contentType);
    }
}