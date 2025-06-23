using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication;
using TheEmployeeAPI.Exceptions;
using TheEmployeeAPI.WebAPI.Extensions;
using TheEmployeeAPI.Extensions;
using TheEmployeeAPI.Infrastructure.DbContexts;
using SharedLibrary;

var builder = WebApplication.CreateBuilder(args);
var conneString = builder.Configuration.GetConnectionString("Default Connection");

builder.Services.AddDbContext<AppDbContext>(options =>
options.UseNpgsql(conneString));

builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddProblemDetails();
builder.Services.AddValidatorsFromAssemblyContaining<Program>();

builder.Services.AddControllers(options =>
{
    options.Filters.Add<FluentValidationFilter>();
}).AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.MaxDepth = 64;
    }); ;
builder.Services.AddSingleton<ISystemClock, SystemClock>();
builder.Services.AddHttpContextAccessor();


builder.Services.ConfigureIdentity();
builder.Services.ConfigureJwt(builder.Configuration);
builder.Services.ConfigureCors(builder.Configuration);

builder.Services
    .AddApplicationServices()
    .AddRepositories()
    .AddMappingProfiles();

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.SwaggerDocumentationExtensions();

var app = builder.Build();

//Application middlewares
app.UseCors("corsPolicy");
app.UseSwaggerDocumentation();
app.UseExceptionHandler();
app.UseHttpsRedirection();
// app.UseMiddleware<RestrictAccessMiddleware>();
app.MapControllers();

await app.InitializeDatabaseAsync();

app.Run();
public partial class Program { }