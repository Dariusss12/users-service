using System.IdentityModel.Tokens.Jwt;
using System.Text;
using DotNetEnv;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using users_service.Src.Consumers;
using users_service.Src.Data;
using users_service.Src.Helpers;
using users_service.Src.Repositories;
using users_service.Src.Repositories.Interfaces;
using users_service.Src.Services;
using users_service.Src.Services.Interfaces;

namespace users_service.Src.Extensions
{
    public static class AppServiceExtensions
    {
        public static void AddApplicationServices(this IServiceCollection services, IConfiguration config)
        {
            InitEnvironmentVariables();
            AddDbContext(services);
            AddServices(services);
            AddAuthentication(services);
            AddGrpc(services);
            AddMassTransit(services);
            
        }

        private static void InitEnvironmentVariables()
        {
            Env.Load();
        }

        private static void AddDbContext(IServiceCollection services)
        {
            // Obtener la cadena de conexión desde el archivo .env
            var connectionUrl = Env.GetString("DB_CONNECTION");

            services.AddDbContext<DataContext>(opt =>
            {
                opt.UseNpgsql(connectionUrl, npgsqlOpt =>
                {
                    npgsqlOpt.EnableRetryOnFailure(
                        maxRetryCount: 10, // Número máximo de intentos
                        maxRetryDelay: TimeSpan.FromSeconds(30), // Retraso máximo entre intentos
                        errorCodesToAdd: null // Puedes agregar códigos específicos de errores de PostgreSQL
                    );
                });
            });
        }
        private static void AddAuthentication(IServiceCollection services)
        {
            var secret = Env.GetString("JWT_SECRET");

            services.AddAuthentication().AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                        secret
                    ))
                };
                options.Events = new JwtBearerEvents
                {
                    OnTokenValidated = context =>
                    {
                        var blacklistService = context.HttpContext.RequestServices.GetRequiredService<IBlacklistService>();

                        if (context.SecurityToken is JwtSecurityToken token && blacklistService.IsBlacklisted(token.RawData))
                        {
                            context.Fail("Token is blacklisted");
                        }

                        return Task.CompletedTask;
                    }
                };
            });
        }

        private static void AddGrpc(IServiceCollection services)
        {
            // Agregar soporte para gRPC
            services.AddGrpc(options =>
            {
                options.Interceptors.Add<BlacklistInterceptor>(); // Añadir el interceptor
            });
        }

        private static void AddMassTransit(IServiceCollection services)
        {
            // Agregar soporte para MassTransit
            services.AddMassTransit(x =>
            {
                x.AddConsumer<CreateUserMessageConsumer>();
                x.AddConsumer<TokenToBlacklistConsumer>();
                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host("localhost", "/", h =>
                    {
                        h.Username("guest");
                        h.Password("guest");
                    });

                    cfg.ReceiveEndpoint("create-user-queue", ep =>
                    {
                        ep.ConfigureConsumer<CreateUserMessageConsumer>(context);
                    });

                    cfg.ReceiveEndpoint("token-blacklist-queue", ep =>
                    {
                        ep.ConfigureConsumer<TokenToBlacklistConsumer>(context);
                    });
                });
            });
        }

        private static void AddServices(IServiceCollection services)
        {
            // Registrar repositorios y servicios
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ISubjectRepository, SubjectRepository>();
            services.AddScoped<IUserService, UserService>();
            services.AddSingleton<IBlacklistService, BlacklistService>();
        }
    }
}