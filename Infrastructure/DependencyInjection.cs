using Application.Interfaces;
using Infrastructure.Authentication;
using Infrastructure.Messaging.Consumers;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<Infrastructure.Persistence.AppDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            services.AddScoped<Domain.Interfaces.IUserRepository, Infrastructure.Persistence.Repositories.UserRepository>();
            services.AddScoped<Domain.Interfaces.ITeamRepository, Infrastructure.Persistence.Repositories.TeamRepository>();
            services.AddScoped<Domain.Interfaces.ITaskRepository, Infrastructure.Persistence.Repositories.TaskRepository>();
            services.AddScoped<Domain.Interfaces.INotificationRepository, Infrastructure.Persistence.Repositories.NotificationRepository>();
            services.AddScoped<Domain.Interfaces.IUnitOfWork, Infrastructure.Persistence.UnitOfWork>();
            services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
            services.AddScoped<IPasswordHasher,PasswordHasher>();
            services.AddScoped<ITaskNotifier, Infrastructure.Services.TaskNotifier>();

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = configuration["JwtSettings:Issuer"],
                    ValidAudience = configuration["JwtSettings:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(configuration["JwtSettings:Secret"]))
                };
            });

            services.AddMassTransit(x =>
            {
                x.AddConsumer<TaskCreatedConsumer>();
                x.AddConsumer<TaskUpdatedConsumer>();


                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host(configuration["MessageBroker:Host"], "/", h =>
                    {
                        h.Username(configuration["MessageBroker:Username"] ?? "guest");
                        h.Password(configuration["MessageBroker:Password"] ?? "guest");
                    });

                    cfg.ConfigureEndpoints(context);
                });
            });

            return services;
        }
    }
}
