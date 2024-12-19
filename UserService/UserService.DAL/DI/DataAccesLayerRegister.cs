﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using UserService.DAL.DataBase;
using UserService.DAL.Interfaces;
using UserService.DAL.Repositories;
using UserService.DAL.UoW;

namespace UserService.DAL.DI
{
    public static class DataAccesLayerRegister
    {
        public static void RegisterDataAccesLayerDapendencies(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(option =>
                option.UseSqlServer(configuration.GetConnectionString(nameof(ApplicationDbContext))));

            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            DatabaseMigrations.ApplyMigrations(services.BuildServiceProvider());
        }
    }
}
