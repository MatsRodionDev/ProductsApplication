using Hangfire;
using UserService.API.Configuration;
using UserService.API.DependencyInjection;
using UserService.API.Extensions;
using UserService.API.Middlewares;
using UserService.BLL.DI;
using UserService.DAL.DI;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddPresentationLayer(builder.Configuration);
builder.Services.RegisterDataAccesLayerDapendencies(builder.Configuration);
builder.Services.RegisteBusinessLogicLayerDapendencies(builder.Configuration);

builder.Host.UseLogging();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseMiddleware<ExceptionHandlingMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseApplyMigrations();
    app.AddJobs(builder.Configuration);
}

app.UseAuthentication();
app.UseAuthorization();

app.UseHangfireDashboard();

app.MapControllers();

app.Run();
