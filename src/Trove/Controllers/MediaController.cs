using Microsoft.AspNetCore.Mvc;
using Serilog.Context;
using Trove.DataModels;
using Trove.Shared;
using Trove.Shared.Repositories;

namespace Trove.Controllers;

[Route("media")]
[ApiController]
public class MediaController : ControllerBase
{
    private readonly IMediaRepository _mediaRepository;
    private readonly ILogger _logger;

    public MediaController(IMediaRepository mediaRepository, ILogger<MediaController> logger)
    {
        _mediaRepository = mediaRepository;
        _logger = logger;
    }

    [HttpGet("{id}")]
    public async Task<Media> GetMedia(Guid id, CancellationToken cancellationToken)
    {
        using (LogContext.PushProperty("MediaId", id))
        {
            Media media = await _mediaRepository.GetMediaAsync(id, cancellationToken);

            return media;
        }
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateMedia([FromBody] Media media, CancellationToken cancellationToken)
    {
        media.Id = null;

        Media newMedia = await _mediaRepository.CreateMediaAsync(media, cancellationToken);

        return CreatedAtAction(nameof(GetMedia), new { id = newMedia.Id }, newMedia);
    }

    [HttpGet("recent")]
    public async Task<IActionResult> ListRecentMedias(CancellationToken cancellationToken)
    {
        return Ok(_mediaRepository.GetRecentMediaAsync(100, cancellationToken));
    }

    [HttpGet]
    public async Task<IActionResult> ListMedias(string? after, CancellationToken cancellationToken)
    {
        Guid? afterGuid = null;

        if (!string.IsNullOrEmpty(after))
        {
            if (SGuid.TryParse(after, out SGuid sguid))
            {
                afterGuid = sguid.Guid;
            }
            else
            {
                return BadRequest();
            }
        }

        return Ok(_mediaRepository.ListMediasAsync(afterGuid, cancellationToken));
    }
}