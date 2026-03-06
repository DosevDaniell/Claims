using Claims.API.BackgroundServices;
using Claims.API.Middleware;
using Claims.Application.Interfaces;
using Claims.Application.Models;
using Claims.Application.Services;
using Claims.Application.Services.Premium;
using Claims.Application.Validation;
using Claims.Infrastructure.Auditing;
using Claims.Infrastructure.Stores;
using System.Threading.Channels;


var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddSingleton<IClock, SystemClock>();
builder.Services.AddSingleton<IClaimStore, InMemoryClaimStore>();
builder.Services.AddScoped<IClaimService, ClaimService>();
builder.Services.AddScoped<ICoverValidator, CoverValidator>();
builder.Services.AddScoped<IClaimValidator, ClaimValidator>();
builder.Services.AddTransient<ExceptionHandlingMiddleware>();
builder.Services.AddSingleton(Channel.CreateUnbounded<AuditEvent>());
builder.Services.AddSingleton<IAuditQueue, InMemoryAuditQueue>();
builder.Services.AddScoped<IAuditWriter, ConsoleAuditWriter>();
builder.Services.AddHostedService<AuditBackgroundService>();
builder.Services.AddScoped<IPremiumCalculator, PremiumCalculator>();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseMiddleware<ExceptionHandlingMiddleware>();

app.MapControllers();

app.MapGet("/", () => Results.Redirect("/swagger"));
app.Run();
public sealed class SystemClock : IClock
{
    public DateTime UtcNow => DateTime.UtcNow;
}