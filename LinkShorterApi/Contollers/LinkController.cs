using Database.Mongo.Models;
using LinkShorter.Domain;
using Microsoft.AspNetCore.Mvc;

namespace LinkShorterApi.Contollers;

[ApiController]
[Route("[controller]")]
public class LinkController : ControllerBase
{
    private readonly LinkService _linkService;

    public LinkController(LinkService linkService)
    {
        _linkService = linkService;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromQuery] string link)
    {
        var result = await _linkService.Create(link);
        return Ok(result);
    }
    
    [HttpGet("GetInformation/{id}")]
    public async Task<IActionResult> GetInformation([FromRoute] string id)
    {
        var result = await _linkService.Get(id);
        return Ok(result);
    }
    
    [HttpGet("{code}")]
    public async Task Move([FromRoute] string code)
    {
        var result = await _linkService.Move(code);
        Response.Redirect(result.MainLink);
    }
}