using OrderService.API.DependencyInjection;
using OrderService.API.Extensions;
using OrderService.API.Middlewares;
using OrderService.Application.Common.DI;
using OrderService.Infrastructure.DI;
using OrderService.Persistence.DI;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddPresentationLayer(builder.Configuration);
builder.Services.AddPersistenceLayer(builder.Configuration);
builder.Services.AddInfrastructureLayer(builder.Configuration);
builder.Services.AddApplicationLayer();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseMiddleware<ExceptionHandlingMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.useApplyMigrations();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
