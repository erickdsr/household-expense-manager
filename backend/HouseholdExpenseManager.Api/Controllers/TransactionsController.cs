using HouseholdExpenseManager.Api.Common;
using HouseholdExpenseManager.Api.DTOs.Transaction;
using HouseholdExpenseManager.Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HouseholdExpenseManager.Api.Controllers;

[ApiController]
[Route("api/transactions")]
public class TransactionsController(ITransactionService transactionService) : ControllerBase
{
    /// <summary>
    /// Retorna todas as transacoes cadastradas incluindo o nome da pessoa.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(List<TransactionResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<List<TransactionResponse>>> GetAllAsync()
    {
        var transactions = await transactionService.GetAllAsync();

        return Ok(transactions);
    }

    /// <summary>
    /// Retorna uma transacao pelo id incluindo o nome da pessoa.
    /// </summary>
    [HttpGet("{id:int}", Name = nameof(GetTransactionByIdAsync))]
    [ProducesResponseType(typeof(TransactionResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<TransactionResponse>> GetTransactionByIdAsync(int id)
    {
        var transaction = await transactionService.GetByIdAsync(id);

        return Ok(transaction);
    }

    /// <summary>
    /// Cria uma transacao para uma pessoa existente.
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(TransactionResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<TransactionResponse>> CreateAsync(CreateTransactionRequest request)
    {
        var transaction = await transactionService.CreateAsync(request);

        return CreatedAtRoute(nameof(GetTransactionByIdAsync), new { id = transaction.Id }, transaction);
    }
}
