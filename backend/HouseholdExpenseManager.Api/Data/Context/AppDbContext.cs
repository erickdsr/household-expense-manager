using HouseholdExpenseManager.Api.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace HouseholdExpenseManager.Api.Data.Context;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    // As propriedades DbSet definem as tabelas gerenciadas pelo EF Core nesta aplicacao.
    public DbSet<Person> People => Set<Person>();

    public DbSet<FinancialTransaction> Transactions => Set<FinancialTransaction>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Person>(entity =>
        {
            entity.ToTable("People");

            // Ao excluir uma pessoa, todas as transacoes relacionadas tambem devem ser excluidas.
            entity.HasMany(person => person.Transactions)
                .WithOne(transaction => transaction.Person)
                .HasForeignKey(transaction => transaction.PersonId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<FinancialTransaction>(entity =>
        {
            entity.ToTable("Transactions");

            // Armazena valores monetarios com duas casas decimais para evitar problemas de arredondamento.
            entity.Property(transaction => transaction.Amount)
                .HasPrecision(18, 2);
        });
    }
}
