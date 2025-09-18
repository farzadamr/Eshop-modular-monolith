using Basket.Data.Repository;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Caching.Distributed;
using Shared.Data;
using Shared.Data.Interceptors;

namespace Basket;

public static class BasketModule
{
    public static IServiceCollection AddBasketModule(this IServiceCollection services
        ,IConfiguration configuration)
    {
        // 1. Api EndPoint Services

        // 2. Application Use Case Services
        services.AddScoped<IBasketRepository, BasketRepository>();
        services.AddScoped<IBasketRepository,CachedBasketRepository>();

        // 3. Data - Infrastructure Services
        var connectionString = configuration.GetConnectionString("Database");

        services.AddScoped<ISaveChangesInterceptor, AuditableEntityInterceptors>();
        services.AddScoped<ISaveChangesInterceptor, DispatchDomainEventsInterceptor>();

        services.AddDbContext<BasketDbContext>((sp, options) =>
        {
            options.AddInterceptors(sp.GetServices<ISaveChangesInterceptor>());
            options.UseNpgsql(connectionString);
        });

        return services;
    }
    public static IApplicationBuilder UseBasketModule(this IApplicationBuilder app)
    {
        // 1. Api EndPoint Services

        // 2. Application Use Case Services

        // 3. Data - Infrastructure Services
        app.UseMigration<BasketDbContext>();
        
        return app;
    }
}
