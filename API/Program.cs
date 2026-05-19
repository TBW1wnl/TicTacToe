using API.Controllers;
using API.Repositories;
using API.Services;

var builder = WebApplication.CreateBuilder(args);

// --- Aspire service defaults (telemetry, health checks, etc.) ---
builder.AddServiceDefaults();

// --- OpenAPI / Swagger ---
builder.Services.AddOpenApi();

// --- Game services ---
// Singleton repository: holds all in-memory game state for the lifetime of the process.
builder.Services.AddSingleton<IGameRepository, InMemoryGameRepository>();

// BotService is stateless, safe as singleton.
builder.Services.AddSingleton<BotService>();

// GameService depends on the two singletons above; singleton is fine here too.
builder.Services.AddSingleton<IGameService, GameService>();

// -----------------------------------------------------------------------

var app = builder.Build();

// --- Aspire default endpoints (health, liveness) ---
app.MapDefaultEndpoints();

// --- Swagger UI in development ---
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

// --- Game endpoints ---
app.MapGameEndpoints();

app.Run();
