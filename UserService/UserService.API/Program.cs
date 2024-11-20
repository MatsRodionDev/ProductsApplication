using FluentValidation;
using FluentValidation.AspNetCore;
using UserService.API.Dtos.Requests;
using UserService.API.Extensions;
using UserService.API.Filters;
using UserService.API.Middlewares;
using UserService.API.Profiles;
using UserService.BLL.Common;
using UserService.BLL.DI;
using UserService.DAL.DI;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection(nameof(JwtOptions)));
builder.Services.Configure<EmailOptions>(builder.Configuration.GetSection(nameof(EmailOptions)));

builder.Services.AddControllers(options => options.Filters
    .Add(typeof(ValidationFilter)));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddFluentValidationAutoValidation()
    .AddFluentValidationClientsideAdapters()
    .AddValidatorsFromAssemblyContaining<LoginUserRequest>();

builder.Services.AddAutoMapper(typeof(ApiProfile));

builder.Services.RegisterDataAccesLayerDapendencies(builder.Configuration);
builder.Services.RegisteBusinessLogicLayerDapendencies(builder.Configuration);

var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.ApplyMigrations();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
