using System.Reflection;
using System.Text.Json.Serialization;
using HouseholdExpenseManager.Api.Data.Context;
using HouseholdExpenseManager.Api.Middlewares;
using HouseholdExpenseManager.Api.Repositories;
using HouseholdExpenseManager.Api.Repositories.Interfaces;
using HouseholdExpenseManager.Api.Services;
using HouseholdExpenseManager.Api.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi;

var builder = WebApplication.CreateBuilder(args);

// Registra as dependencias da aplicacao por camada: dados, regras de negocio e resumos.
builder.Services.AddDbContext<AppDbContext>(options =>
    // SQLite mantem os dados em um arquivo local para preservar os registros apos fechar a aplicacao.
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IPersonRepository, PersonRepository>();
builder.Services.AddScoped<ITransactionRepository, TransactionRepository>();
builder.Services.AddScoped<IPersonService, PersonService>();
builder.Services.AddScoped<ITransactionService, TransactionService>();
builder.Services.AddScoped<ISummaryService, SummaryService>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("http://localhost:5173", "http://127.0.0.1:5173")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        // Mantem os valores de enum legiveis no contrato da API, por exemplo "Income" em vez de 1.
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter(allowIntegerValues: false));
    });
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Household Expense Manager API",
        Version = "v1",
        Description = "API para gerenciar pessoas, transacoes financeiras domesticas e resumos de despesas."
    });

    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    options.IncludeXmlComments(xmlPath);
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    // Aplica migracoes pendentes automaticamente para alinhar o banco local ao modelo atual.
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    context.Database.Migrate();
}

app.UseCors("AllowFrontend");

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    // O Swagger UI fica na raiz da API para facilitar a exploracao local.
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Household Expense Manager API v1");
    c.RoutePrefix = string.Empty;
});

app.UseMiddleware<ErrorHandlingMiddleware>();

app.MapControllers();

app.Run();
