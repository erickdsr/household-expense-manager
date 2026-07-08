using System.ComponentModel.DataAnnotations;
using HouseholdExpenseManager.Api.DTOs.Transaction;
using HouseholdExpenseManager.Api.Exceptions;
using HouseholdExpenseManager.Api.Models.Entities;
using HouseholdExpenseManager.Api.Models.Enums;
using HouseholdExpenseManager.Api.Repositories.Interfaces;
using HouseholdExpenseManager.Api.Services.Interfaces;

namespace HouseholdExpenseManager.Api.Services;

// Aplica validacoes e regras de negocio especificas de transacoes antes da persistencia.
public class TransactionService(
    ITransactionRepository transactionRepository,
    IPersonRepository personRepository) : ITransactionService
{
    public async Task<List<TransactionResponse>> GetAllAsync()
    {
        var transactions = await transactionRepository.GetAllAsync();

        return transactions.Select(MapToResponse).ToList();
    }

    public async Task<TransactionResponse> GetByIdAsync(int id)
    {
        var transaction = await transactionRepository.GetByIdAsync(id);

        if (transaction is null)
        {
            throw new NotFoundException("Transaction not found.");
        }

        return MapToResponse(transaction);
    }

    public async Task<TransactionResponse> CreateAsync(CreateTransactionRequest request)
    {
        // Normaliza a descricao para que espacos em branco nao sejam salvos como texto valido.
        var description = request.Description.Trim();

        if (string.IsNullOrWhiteSpace(description))
        {
            throw new ValidationException("Description is required.");
        }

        if (request.Amount <= 0)
        {
            throw new ValidationException("Amount must be greater than zero.");
        }

        if (request.Type is null)
        {
            throw new ValidationException("Transaction type is required.");
        }

        var person = await personRepository.GetByIdAsync(request.PersonId);

        if (person is null)
        {
            throw new NotFoundException("Person not found.");
        }

        // Menores de idade podem registrar apenas despesas, nunca renda.
        if (person.Age < 18 && request.Type == TransactionType.Income)
        {
            throw new BusinessRuleException("Minors can only register expense transactions.");
        }

        var transaction = new FinancialTransaction
        {
            Description = description,
            Amount = request.Amount,
            Type = request.Type.Value,
            PersonId = request.PersonId,
            CreatedAt = DateTime.UtcNow
        };

        var createdTransaction = await transactionRepository.CreateAsync(transaction);

        return MapToResponse(createdTransaction, person.Name);
    }

    private static TransactionResponse MapToResponse(FinancialTransaction transaction)
    {
        return MapToResponse(transaction, transaction.Person.Name);
    }

    private static TransactionResponse MapToResponse(FinancialTransaction transaction, string personName)
    {
        return new TransactionResponse
        {
            Id = transaction.Id,
            Description = transaction.Description,
            Amount = transaction.Amount,
            Type = transaction.Type,
            PersonId = transaction.PersonId,
            PersonName = personName,
            CreatedAt = transaction.CreatedAt
        };
    }
}
