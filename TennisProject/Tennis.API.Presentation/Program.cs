using Tennis.API.Infrastructure.Interfaces;
using Tennis.API.Infrastructure;
using Tennis.API.Services.Interfaces;
using Tennis.API.Services;
using Microsoft.OpenApi.Models;
using Tennis.API.Shared.Options;

var builder = WebApplication.CreateBuilder(args);
// Bind Options
builder.Services.Configure<PlayerApiOptions>(
    builder.Configuration.GetSection(PlayerApiOptions.SectionName));

// Add services to the container.

builder.Services.AddHttpClient();
builder.Services.AddScoped<IPlayerRepository, PlayerRepository>();
builder.Services.AddScoped<IPlayerService, PlayerService>();
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Tennis Stats API",
        Version = "v1",
        Description = "API to manage tennis player statistics"

    });
});

var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.MapGet("/", () => "Tennis API is running");
app.Run();
