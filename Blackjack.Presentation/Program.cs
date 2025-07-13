using Blackjack.Business.BackgroundServices;
using Blackjack.Business.Services;
using Blackjack.Business.Services.Interfaces;
using Blackjack.Data.Context;
using Blackjack.Data.Interfaces;
using Blackjack.Data.Other.Handlers;
using Blackjack.Data.Other.Handlers.Base;
using Blackjack.Data.Repositories;
using Blackjack.Data.Repositories.Interfaces;
using Blackjack.Presentation.Hubs;
using Blackjack.Presentation.Middlewares;
using Blackjack.Presentation.Other.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSignalR();
builder.Services.AddLogging();
builder.Services.AddControllers();
builder.Services.AddMemoryCache();

builder.Services.AddDbContextFactory<DatabaseContext>(options => 
    options.UseNpgsql(builder.Configuration.GetConnectionString("ContainerDatabase"), options =>
    {
        options.MigrationsAssembly("Blackjack.Data");
    }));

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        policy =>
        {
            policy.SetIsOriginAllowed(_ => true)
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials();
        });
});
builder.Services.AddScoped<ITransactionHandler, TransactionHandler>();

builder.Services.AddScoped<IPlayerConnectionRepository, PlayerConnectionRepository>();
builder.Services.AddScoped<GameRepository>();
builder.Services.AddScoped<IGameRepository, CachedGameRepository>(sp =>
{
    var repo = sp.GetRequiredService<GameRepository>();
    var cache = sp.GetRequiredService<IMemoryCache>();
    var logger = sp.GetRequiredService<ILogger<CachedGameRepository>>();
    return new CachedGameRepository(repo, cache, logger);
});
builder.Services.AddScoped<IPlayerRepository, PlayerRepository>();

builder.Services.AddScoped<IPlayerConnectionService, PlayerConnectionService>();
builder.Services.AddScoped<IGameService, GameService>();
builder.Services.AddScoped<IPlayerService, PlayerService>();
builder.Services.AddSingleton<IGameHubDispatcher, GameHubDispatcher>();
builder.Services.AddScoped<IGameHubService, GameHubService>();

/*
builder.Services.AddHostedService<GameCleanupService>();
*/

var app = builder.Build();
app.ApplyMigrations();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}
app.UseCors("AllowAllOrigins");
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseCors();
app.UseAuthorization();
app.UseMiddleware<ExceptionHandler>();
app.MapControllers();

app.MapHub<GameHub>("/gameHub");

app.Run();

