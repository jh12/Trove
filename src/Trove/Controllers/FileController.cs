using System.Net;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Serilog.Context;
using Trove.Shared;
using Trove.Shared.Repositories;
using FileOptions = Trove.Shared.Options.FileOptions;
using FileResult = Trove.Shared.Models.FileResult;

namespace Trove.Controllers;

[Route("file")]
[ApiController]
public class FileController : ControllerBase
{
    private readonly IFileRepository _fileRepository;
    private readonly IMediaRepository _mediaRepository;
    private readonly FileOptions _fileOptions;
    private readonly ILogger<FileController> _logger;

    public FileController(IFileRepository fileRepository, IMediaRepository mediaRepository, IOptionsSnapshot<FileOptions> fileOptionsSnapshot, ILogger<FileController> logger)
    {
        _fileRepository = fileRepository;
        _mediaRepository = mediaRepository;
        _fileOptions = fileOptionsSnapshot.Value;
        _logger = logger;
    }

    [HttpGet("{id}")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType(400)]
    public async Task<ActionResult> GetFile(string id, CancellationToken cancellationToken)
    {
        if (!SGuid.TryParse(id, out SGuid sguid))
            return BadRequest();

        FileResult fileResult = await _fileRepository.GetFileAsync(sguid.Guid, cancellationToken);

        return new FileStreamResult(fileResult.Stream, fileResult.ContentType);
    }

    [HttpPost("{id}")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    public async Task<ActionResult> UploadFile(string id, CancellationToken cancellationToken)
    {
        if (!SGuid.TryParse(id, out SGuid sguid))
            return BadRequest();

        using (LogContext.PushProperty("MediaId", sguid.Guid))
        {
            if (Request.ContentLength > _fileOptions.Upload.MaxFileSizeBytes)
                return new UnprocessableEntityResult();

            if (!await _mediaRepository.MediaExistsAsync(sguid.Guid, cancellationToken))
                return BadRequest();

            SHA256 hashAlgorithm = SHA256.Create();

            await using (CryptoStream hashStream = new(Request.Body, hashAlgorithm, CryptoStreamMode.Read, false))
            {
                await _fileRepository.SaveFileAsync(sguid.Guid, hashStream, cancellationToken);

                if (!hashStream.HasFlushedFinalBlock)
                    await hashStream.FlushFinalBlockAsync(cancellationToken);
            }

            return Ok();
        }
    }
}