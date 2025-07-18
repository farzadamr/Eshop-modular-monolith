using Carter;
using Shared.Extentions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container

builder.Services
    .AddCarterWithAssemblies();

builder.Services
    .AddCatalogModule(builder.Configuration)
    .AddBasketModule(builder.Configuration)
    .AddOrderingModule(builder.Configuration);
var app = builder.Build();


// Configure the HTTP request pipeline

app.MapCarter();

app
    .UseCatalogModule()
    .UseBasketModule()
    .UseOrderingModule();

app.Run();
