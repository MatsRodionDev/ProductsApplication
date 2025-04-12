﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using UserService.DAL.Interceptors;
using UserService.DAL.Interfaces;
using UserService.DAL.Repositories;
using UserService.DAL.UoW;

namespace UserService.DAL.DI
{
    public static class DataAccesLayerRegister
    {
        public static void RegisterDataAccesLayerDapendencies(this IServiceCollection services, IConfiguration configuration)
        {
            var con = configuration.GetConnectionString(nameof(ApplicationDbContext));

            services.AddDbContext<ApplicationDbContext>(options =>
                 options.UseNpgsql(configuration.GetConnectionString(nameof(ApplicationDbContext)))
                    .AddInterceptors(new CreateAuditableInterceptor()));

            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
        }
    }
}

