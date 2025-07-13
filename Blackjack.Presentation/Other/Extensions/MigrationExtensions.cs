using Blackjack.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace Blackjack.Presentation.Other.Extensions;

public static class MigrationExtensions
{
    public static async void CreateDatabase(this IApplicationBuilder app)
    {
        await using var serviceScope = app.ApplicationServices.CreateAsyncScope();

        DatabaseContext context =
            serviceScope.ServiceProvider.GetRequiredService<DatabaseContext>();
        
        await context.Database.MigrateAsync();
    }
}