using System.Security.Cryptography;
using DotNetEnv;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using users_service.Src.Data;
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
            //AddAuthentication(services);
            AddGrpc(services);
            AddServices(services);
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

/*
        private static void AddAuthentication(IServiceCollection services)
        {
            // Configurar autenticación JWT usando la clave pública
            var publicKeyPath = Env.GetString("JWT_PUBLIC_KEY_PATH");

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = Env.GetString("JWT_ISSUER"),
                        ValidAudience = Env.GetString("JWT_AUDIENCE"),
                        IssuerSigningKey = new RsaSecurityKey(GetRsaPublicKey(publicKeyPath))
                    };
                });
        }

        private static RSA GetRsaPublicKey(string publicKeyPath)
        {
            // Cargar la clave pública desde el archivo especificado
            var publicKeyPem = File.ReadAllText(publicKeyPath);
            var rsa = RSA.Create();
            rsa.ImportFromPem(publicKeyPem);
            return rsa;
        }
        */

        private static void AddGrpc(IServiceCollection services)
        {
            // Agregar soporte para gRPC
            services.AddGrpc(options =>
            {
                options.MaxReceiveMessageSize = 20 * 1024 * 1024; // 20 MB, puedes ajustarlo según sea necesario
                options.MaxSendMessageSize = 20 * 1024 * 1024;    // 20 MB
            });
        }

        private static void AddServices(IServiceCollection services)
        {
            // Registrar repositorios y servicios
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ISubjectRepository, SubjectRepository>();
            services.AddScoped<IUserService, UserService>();
        }
    }
}