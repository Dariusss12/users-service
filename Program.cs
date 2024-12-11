using users_service.Src.Extensions;

var builder = WebApplication.CreateBuilder(args);

var port = Environment.GetEnvironmentVariable("PORT") ?? "5000";  // Si no se encuentra la variable, usa 5000 como predeterminado

// Configurar Kestrel para escuchar en el puerto de Render (o el puerto por defecto)
builder.WebHost.UseKestrel(options =>
{
    options.ListenAnyIP(int.Parse(port), listenOptions =>
    {
        listenOptions.Protocols = Microsoft.AspNetCore.Server.Kestrel.Core.HttpProtocols.Http2; // gRPC necesita HTTP/2
    });
});

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddApplicationServices(builder.Configuration);


var app = builder.Build();

//app.UseAuthentication();
//app.UseAuthorization();
app.MapGrpcService<UserServiceGrpc>();
app.MapControllers();

AppSeedService.SeedDatabase(app);

app.Run();
