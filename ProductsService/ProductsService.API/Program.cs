using ProductsService.Persistence.DI;
using ProductsService.Infrastructure.DI;
using ProductsService.Application.Common.DI;
using ProductsService.API.Middlewares;
using ProductsService.API.DI;
using ProductsService.API.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddPresentationLayer(builder.Configuration);
builder.Services.AddPersistenceLayer(builder.Configuration);
builder.Services.AddInfrastructureLayer(builder.Configuration);
builder.Services.AddApplicationLayer(builder.Configuration);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
    
var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseMiddleware<ExceptionHandlingMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.MapGrpcReflectionService();
app.MapGrpcService<ProductGrpcService>();

app.Run();
