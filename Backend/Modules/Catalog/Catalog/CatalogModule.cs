using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.Behaviors;
using Shared.Data.Interceptors;

namespace Catalog;
public static class CatalogModule
{
    public static IServiceCollection AddCatalogModule(this IServiceCollection services,
        IConfiguration configuration)
    {
        //Application Use Case Services
        services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            config.AddOpenBehavior(typeof(ValidationBehavior<,>));
            config.AddOpenBehavior(typeof(LoggingBehavior<,>));
        });

        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());


        // Infrastructure Services
        var connectionString = configuration.GetConnectionString("Database");

        services.AddScoped<ISaveChangesInterceptor, AuditableEntityInterceptors>();
        services.AddScoped<ISaveChangesInterceptor, DispatchDomainEventsInterceptor>();


        services.AddDbContext<CatalogDbContext>((sp ,options) =>
        {
            options.AddInterceptors(sp.GetServices<ISaveChangesInterceptor>());
            options.UseNpgsql(connectionString);
        });

        services.AddScoped<IDataSeeder, CatalogDataSeeder>();
        
        return services;
    }

    public static IApplicationBuilder UseCatalogModule(this IApplicationBuilder app)
    {
        app.UseMigration<CatalogDbContext>();
        return app;
    }

}
