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
        var result = await _genreService.GetAllGenresAsync();
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _genreService.GetGenreByIdAsync(id);
        if (!result.IsSuccess) return BadRequest(result);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateGenreDto dto)
    {
        var result = await _genreService.CreateGenreAsync(dto);
        if (!result.IsSuccess) return BadRequest(result);

        return Ok(result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, CreateGenreDto dto)
    {
        var result = await _genreService.UpdateGenreAsync(id, dto);
        if (!result.IsSuccess) return BadRequest(result);

        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _genreService.DeleteGenreAsync(id);
        
        if (!result.IsSuccess) return NotFound(result);

        return Ok(result);
    }
}