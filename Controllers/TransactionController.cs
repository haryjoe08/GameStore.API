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
        var transactions = await _transactionService.GetAllAsync();
    
        return Ok(new
        {
            Message = "Berhasil mendapatkan list transaksi!",
            Data = transactions
        });
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var transaction = await _transactionService.GetByIdAsync(id);
      
        return Ok(new
        {
            Message = "Transaksi ditemukan!",
            Data = transaction
        });
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateTransactionDto dto)
    {
        var createdTransaction = await _transactionService.CreateAsync(dto);

        if (createdTransaction == null)
        {
            return BadRequest(new { Message = "GameId tidak valid atau stok game tidak mencukupi." });
        }

        return Ok(new
        {
            Message = "Transaksi berhasil.",
            Data = createdTransaction
        });
    }
}