using GameStoreApi.DTOs;
using GameStoreApi.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GameStoreApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PublishersController : ControllerBase
{
    private readonly IPublisherService _publisherService;

    public PublishersController(IPublisherService publisherService)
    {
        _publisherService = publisherService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _publisherService.GetAllPublishersAsync();
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _publisherService.GetPublisherByIdAsync(id);
        if(!result.IsSuccess) return BadRequest(result);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreatePublisherDto dto)
    {
        var result = await _publisherService.CreatePublisherAsync(dto);
        if (!result.IsSuccess) return BadRequest(result);

        return Ok(result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, CreatePublisherDto dto)
    {
        var result = await _publisherService.UpdatePublisherAsync(id, dto);
        if (!result.IsSuccess) return BadRequest(result);

        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _publisherService.DeletePublisherAsync(id);
        if (!result.IsSuccess) return BadRequest(result);
        return Ok(result);
    }
}