using Carter;
using Microsoft.AspNetCore.Builder;
using Shared.Exceptions.Handlers;
using Shared.Extentions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container

builder.Services
    .AddCarterWithAssemblies(
        typeof(CatalogModule).Assembly,
        typeof(BasketModule).Assembly,
        typeof(OrderingModule).Assembly
    );

builder.Services
    .AddCatalogModule(builder.Configuration)
    .AddBasketModule(builder.Configuration)
    .AddOrderingModule(builder.Configuration);
builder.Services.AddExceptionHandler<CustomExceptionHandler>();

var app = builder.Build();

// Configure the HTTP request pipeline
app.UseExceptionHandler(options => { });

app
    .UseCatalogModule()
    .UseBasketModule()
    .UseOrderingModule();


app.MapCarter();

app.MapGet("/api/health", () => { return "Hello, From App!!"; });


app.Run();
