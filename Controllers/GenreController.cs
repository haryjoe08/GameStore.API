using GameStoreApi.DTOs;
using GameStoreApi.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GameStoreApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GenresController : ControllerBase
{
    private readonly IGenreService _genreService;

    public GenresController(IGenreService genreService)
    {
        _genreService = genreService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var genres = await _genreService.GetAllAsync();
        return Ok(genres);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var genre = await _genreService.GetByIdAsync(id);
        if (genre == null) return NotFound("Genre tidak ditemukan.");
        return Ok(genre);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateGenreDto dto)
    {
        var createdGenre = await _genreService.CreateAsync(dto);
        if (createdGenre == null)
        {
            return BadRequest("Gagal Menambahkan Genre.");
        }

        return Ok(new
        {
            Message = "Genre berhasil ditambahkan!",
            Data = createdGenre
        });
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, CreateGenreDto dto)
    {
        var updatedGenre = await _genreService.UpdateAsync(id, dto);
        if (updatedGenre == null) return NotFound("Genre tidak ditemukan.");

        return Ok(new
        {
            Message = "Genre berhasil diperbarui!",
            Data = updatedGenre
        });
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _genreService.DeleteAsync(id);

        if (result == null)
        {
            return NotFound(new { Message = "Genre tidak ditemukan." });
        }

        if (result == false)
        {
            return BadRequest(new
                { Message = "Genre tidak dapat dihapus karena masih digunakan oleh satu atau lebih Game." });
        }

        return Ok(new { Message = "Genre berhasil dihapus." });
    }
}