using HouseholdExpenseManager.Api.Data.Context;
using HouseholdExpenseManager.Api.Repositories;
using HouseholdExpenseManager.Api.Repositories.Interfaces;
using HouseholdExpenseManager.Api.Services;
using HouseholdExpenseManager.Api.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IPersonRepository, PersonRepository>();
builder.Services.AddScoped<ITransactionRepository, TransactionRepository>();
builder.Services.AddScoped<IPersonService, PersonService>();

builder.Services.AddControllers();

var app = builder.Build();

app.MapControllers();

app.Run();
