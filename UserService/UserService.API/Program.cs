using UserService.API.DependencyInjection;
using UserService.API.Middlewares;
using UserService.BLL.DI;
using UserService.DAL.DI;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddPresentationLayer(builder.Configuration);
builder.Services.RegisterDataAccesLayerDapendencies(builder.Configuration);
builder.Services.RegisteBusinessLogicLayerDapendencies(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseHttpsRedirection();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
