using GameStoreApi.DTOs;
using GameStoreApi.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GameStoreApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TransactionsController : ControllerBase
{
    private readonly ITransactionService _transactionService;

    public TransactionsController(ITransactionService transactionService)
    {
        _transactionService = transactionService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _transactionService.GetAllTransactionsAsync();
    
        
        if(!result.IsSuccess) return BadRequest(result);

        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _transactionService.GetTransactionByIdAsync(id);
      
        
        if(!result.IsSuccess) return BadRequest(result);

        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateTransactionDto dto)
    {
        var result = await _transactionService.CreateTransactionAsync(dto);

        if(!result.IsSuccess) return BadRequest(result);

        return Ok(result);
    }
}