using Microsoft.EntityFrameworkCore;

namespace webbuilder.api.data
{
    public static class DataExtension
    {
        public static void MigrateDB(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ElementStoreContext>();
            context.Database.Migrate();
        }

    }
}