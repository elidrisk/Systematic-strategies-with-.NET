using GrpcBacktestServer.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddGrpc();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapGrpcService<BacktestService>();  // Utilisez ici le service correspondant à votre fichier proto

app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client.");

app.Run();
