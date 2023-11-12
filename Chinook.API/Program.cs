using Chinook.API.Configurations;
using Chinook.Domain.Helpers;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAppSettings(builder.Configuration);
builder.Services.AddConnectionProvider(builder.Configuration);
builder.Services.ConfigureRepositories();
builder.Services.ConfigureSupervisor();
builder.Services.AddAPILogging();
builder.Services.AddCORS();
builder.Services.ConfigureValidators();
builder.Services.AddCaching(builder.Configuration);
builder.Services.AddHypermedia();

builder.Services.AddControllers(cfg =>
{
    cfg.Filters.Add<RepresentationEnricher>();
});

var app = builder.Build();

app.UseHttpLogging();
app.UseHttpsRedirection();

app.UseCors();

app.UseResponseCaching();

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
