using Blackjack.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace Blackjack.Presentation.Other.Extensions;

public static class MigrationExtensions
{
    public static void ApplyMigrations(this IApplicationBuilder app)
    {
        using IServiceScope serviceScope = app.ApplicationServices.CreateScope();
        
        using DatabaseContext context = 
            serviceScope.ServiceProvider.GetRequiredService<DatabaseContext>();
        
        context.Database.Migrate();
    }
}