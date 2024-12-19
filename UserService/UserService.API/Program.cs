using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Shared.Consts;
using Shared.Enums;
using System.Text;
using UserService.API.Authorization;
using UserService.API.Dtos.Requests;
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

builder.Services.AddAutoMapper(typeof(UserProfile));

builder.Services.RegisterDataAccesLayerDapendencies(builder.Configuration);
builder.Services.RegisteBusinessLogicLayerDapendencies(builder.Configuration);

var jwtOptions = builder.Configuration.GetSection(nameof(JwtOptions)).Get<JwtOptions>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
    {
        options.TokenValidationParameters = new()
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions!.SecretKey))
        };

        options.Events = new JwtBearerEvents()
        {
            OnMessageReceived = context =>
            {
                context.Token = context.Request.Cookies[CookiesConstants.ACCESS];

                return Task.CompletedTask;
            }
        };
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy(Policies.ADMIN, policy =>
    {
        policy.AddRequirements(new RoleRequirment(RoleEnum.Admin));
    });


    options.AddPolicy(Policies.USER, policy =>
    {
        policy.AddRequirements(new RoleRequirment(RoleEnum.User));
    });
});

builder.Services.AddSingleton<IAuthorizationHandler, RoleRequirmentHandler>();

var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
