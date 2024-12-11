using users_service.Src.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddApplicationServices(builder.Configuration);


var app = builder.Build();


app.UseAuthorization();
app.MapGrpcService<UserServiceGrpc>();
app.MapGet("/", () => "Communication with gRPC!");

AppSeedService.SeedDatabase(app);

app.Run();
