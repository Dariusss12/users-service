using users_service.Src.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.ConfigureKestrel((context, options) =>
{
    // Cargar la configuración de Kestrel desde appsettings.json
    options.Configure(context.Configuration.GetSection("Kestrel"));
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
app.MapGet("/", () => "Communication with gRPC!");

AppSeedService.SeedDatabase(app);

app.Run();
