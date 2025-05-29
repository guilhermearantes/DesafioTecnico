using DesafioTecnicoObjective.Services;
using DesafioTecnicoObjective.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Adiciona os servi�os necess�rios
builder.Services.AddControllers();
builder.Services.AddScoped<IContaRepository, ContaRepository>();
builder.Services.AddScoped<IContaService, ContaService>();

// Configura o DbContext principal
builder.Services.AddDbContext<ContaDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// Middleware global de tratamento de exce��es
app.UseMiddleware<DesafioTecnicoObjective.Exceptions.ExceptionMiddleware>();

app.MapControllers();

// Aplica migra��es para o contexto principal
using (var scope = app.Services.CreateScope())
{
    var contaDb = scope.ServiceProvider.GetRequiredService<ContaDbContext>();
    contaDb.Database.Migrate();
}

await app.RunAsync();
