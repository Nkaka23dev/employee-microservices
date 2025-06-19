using Microsoft.EntityFrameworkCore;
using PayrollService.Data;
using PayrollService.SyncDataService.Http;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddDbContext<AppDbContext>(option => option.UseInMemoryDatabase("inMem"));
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddControllers();
builder.Services.AddHttpClient<IHttpTimeTrackingDataClient, HttpTimeTrackingDataClient>();
builder.Services.AddScoped<IPayrollRepo, PayrollRepo>();

var timeTrackingServiceUrl = builder.Configuration["TimeTrackingService"];
Console.WriteLine($"TimeTrackingService URL: {timeTrackingServiceUrl}");

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseHttpsRedirection(); // keep this in dev only
}

// app.UseHttpsRedirection(); // optional: comment out for Docker if needed

app.MapControllers();

PrepDb.PrepPopulation(app);
app.Run();
