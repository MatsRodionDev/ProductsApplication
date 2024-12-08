using ProductsService.Application.Common;
using ProductsService.Persistence.DI;
using ProductsService.Infrastructure.DI;
using ProductsService.Application.Common.DI;
using ProductsService.API.Middlewares;
using ProductsService.Infrastructure.MessageBroker;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddPersistenceLayer(builder.Configuration);
builder.Services.Configure<MinioOptions>(builder.Configuration.GetSection(nameof(MinioOptions)));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

builder.Services.Configure<MessageBrokerSettings>(
    builder.Configuration.GetSection(nameof(MessageBrokerSettings)));
builder.Services.AddSingleton(sp =>
    sp.GetRequiredService<IOptions<MessageBrokerSettings>>().Value);

builder.Services.AddInfrastructureLayer(builder.Configuration);
builder.Services.AddApplicationLayer(builder.Configuration);

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
