using ProductsService.Persistence.DI;
using ProductsService.Infrastructure.DI;
using ProductsService.Application.Common.DI;
using ProductsService.API.Middlewares;
using ProductsService.API.DI;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddPresentationLayer(builder.Configuration);
builder.Services.AddPersistenceLayer(builder.Configuration);
builder.Services.AddInfrastructureLayer(builder.Configuration);
builder.Services.AddApplicationLayer(builder.Configuration);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
