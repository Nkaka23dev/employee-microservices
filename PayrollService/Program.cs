using Microsoft.EntityFrameworkCore;
using PayrollService.Data;
using PayrollService.SyncDataService.Http;

var builder = WebApplication.CreateBuilder(args);

// Register common services
builder.Services.AddOpenApi();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddControllers();
builder.Services.AddHttpClient<IHttpTimeTrackingDataClient, HttpTimeTrackingDataClient>();
builder.Services.AddScoped<IPayrollRepo, PayrollRepo>();

// Determining environment before calling builder.Build()
if (builder.Environment.IsProduction())
{
    Console.WriteLine("=====> Using SqlServerdb for production");
    builder.Services.AddDbContext<AppDbContext>(option =>
        option.UseSqlServer(builder.Configuration.GetConnectionString("PayrollsConn")));
}
else
{
    Console.WriteLine("=====> Using InMem db");
    builder.Services.AddDbContext<AppDbContext>(option =>
        option.UseInMemoryDatabase("inMem"));
}

var app = builder.Build();

// Middleware
if (!app.Environment.IsProduction())
{
    app.MapOpenApi();
    app.UseHttpsRedirection();
}

app.MapControllers();
PrepDb.PrepPopulation(app, app.Environment.IsProduction());
app.Run();
