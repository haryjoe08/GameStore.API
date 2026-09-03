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
        var publishers = await _publisherService.GetAllAsync();
        return Ok(publishers);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var publisher = await _publisherService.GetByIdAsync(id);
        if (publisher == null) return NotFound("Publisher tidak ditemukan.");
        return Ok(publisher);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreatePublisherDto dto)
    {
        var createdPublisher = await _publisherService.CreateAsync(dto);
        if (createdPublisher == null)
        {
            return BadRequest("Gagal Menambahkan Publisher.");
        }

        return Ok(new
        {
            Message = "Publisher berhasil ditambahkan!",
            Data = createdPublisher
        });
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, CreatePublisherDto dto)
    {
        var updatedPublisher = await _publisherService.UpdateAsync(id, dto);
        if (updatedPublisher == null)
        {
            return NotFound(new { Message = "Publisher tidak ditemukan." });
        }

        return Ok(new
        {
            Message = "Publisher berhasil diperbarui!",
            Data = updatedPublisher
        });
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _publisherService.DeleteAsync(id);

        if (result == null)
        {
            return NotFound(new { Message = "Publisher tidak ditemukan." });
        }

        if (result == false)
        {
            return BadRequest(new
                { Message = "Publisher tidak dapat dihapus karena masih digunakan oleh satu atau lebih Game." });
        }

        return Ok(new { Message = "Publisher berhasil dihapus." });
    }
}