using System.Text;
using Chinook.Data;
using Chinook.Domain.ApiModels;
using Chinook.Domain.Repositories;
using Chinook.Domain.Supervisor;
using Chinook.Domain.Validation;
using FluentValidation;
using FluentValidation.AspNetCore;
using Hellang.Middleware.ProblemDetails;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using Microsoft.AspNetCore.HttpLogging;
using Chinook.Data.Repositories;
using Chinook.Domain.Enrichers;
using Chinook.Domain.Helpers;

namespace Chinook.API.Configurations;

public static class ServicesConfiguration
{
    public static void ConfigureRepositories(this IServiceCollection services)
    {
        services.AddScoped<IAlbumRepository, AlbumRepository>()
            .AddScoped<IArtistRepository, ArtistRepository>()
            .AddScoped<ICustomerRepository, CustomerRepository>()
            .AddScoped<IEmployeeRepository, EmployeeRepository>()
            .AddScoped<IGenreRepository, GenreRepository>()
            .AddScoped<IInvoiceRepository, InvoiceRepository>()
            .AddScoped<IInvoiceLineRepository, InvoiceLineRepository>()
            .AddScoped<IMediaTypeRepository, MediaTypeRepository>()
            .AddScoped<IPlaylistRepository, PlaylistRepository>()
            .AddScoped<ITrackRepository, TrackRepository>();
    }

    public static void ConfigureSupervisor(this IServiceCollection services)
    {
        services.AddScoped<IChinookSupervisor, ChinookSupervisor>();
    }

    public static void AddAPILogging(this IServiceCollection services)
    {
        services.AddLogging(builder => builder
            .AddConsole()
            .AddFilter(level => level >= LogLevel.Information)
        );
        
        services.AddHttpLogging(logging =>
        {
            // Customize HTTP logging.
            logging.LoggingFields = HttpLoggingFields.All;
            logging.RequestHeaders.Add("My-Request-Header");
            logging.ResponseHeaders.Add("My-Response-Header");
            logging.MediaTypeOptions.AddText("application/javascript");
            logging.RequestBodyLogLimit = 4096;
            logging.ResponseBodyLogLimit = 4096;
        });
    }

    public static void ConfigureValidators(this IServiceCollection services)
    {
        services.AddFluentValidationAutoValidation()
            .AddTransient<IValidator<AlbumApiModel>, AlbumValidator>()
            .AddTransient<IValidator<ArtistApiModel>, ArtistValidator>()
            .AddTransient<IValidator<CustomerApiModel>, CustomerValidator>()
            .AddTransient<IValidator<EmployeeApiModel>, EmployeeValidator>()
            .AddTransient<IValidator<GenreApiModel>, GenreValidator>()
            .AddTransient<IValidator<InvoiceApiModel>, InvoiceValidator>()
            .AddTransient<IValidator<InvoiceLineApiModel>, InvoiceLineValidator>()
            .AddTransient<IValidator<MediaTypeApiModel>, MediaTypeValidator>()
            .AddTransient<IValidator<PlaylistApiModel>, PlaylistValidator>()
            .AddTransient<IValidator<TrackApiModel>, TrackValidator>();
    }

    public static void AddCORS(this IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddPolicy("CorsPolicy",
                builder => builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader());
        });
    }

    public static void AddCaching(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddResponseCaching();
        services.AddMemoryCache();
        services.AddDistributedSqlServerCache(options =>
        {
            options.ConnectionString = configuration.GetConnectionString("ChinookSQLCache");
            options.SchemaName = "dbo";
            options.TableName = "ChinookCache";
        });
    }

    public static void AddSwaggerServices(this IServiceCollection services)
    {
        services.AddSwaggerGen();
        services.ConfigureOptions<ConfigureSwaggerOptions>();
    }

    public static void AddApiExplorer(this IServiceCollection services)
    {
        services.AddVersionedApiExplorer(setup =>
        {
            setup.GroupNameFormat = "'v'VVV";
            setup.SubstituteApiVersionInUrl = true;
        });
    }

    public static void AddProblemDetail(this IServiceCollection services)
    {
        services.AddProblemDetails(opts =>
            {
                // Control when an exception is included
                opts.IncludeExceptionDetails = (ctx, _) =>
                {
                    // Fetch services from HttpContext.RequestServices
                    var env = ctx.RequestServices.GetRequiredService<IHostEnvironment>();
                    return env.IsDevelopment() || env.IsStaging();
                };
            }
        );
    }
    
    public static void AddRepresentations(this IServiceCollection services)
    {
        services.AddHttpContextAccessor();
        
        services.AddScoped<AlbumEnricher>()
            .AddScoped<IEnricher, AlbumEnricher>()
            .AddScoped<AlbumsEnricher>()
            .AddScoped<IListEnricher, AlbumsEnricher>()
            .AddScoped<ArtistEnricher>()
            .AddScoped<IEnricher, ArtistEnricher>()
            .AddScoped<ArtistsEnricher>()
            .AddScoped<IListEnricher, ArtistsEnricher>()
            .AddScoped<CustomerEnricher>()
            .AddScoped<IEnricher, CustomerEnricher>()
            .AddScoped<CustomersEnricher>()
            .AddScoped<IListEnricher, CustomersEnricher>()
            .AddScoped<EmployeeEnricher>()
            .AddScoped<IEnricher, EmployeeEnricher>()
            .AddScoped<EmployeesEnricher>()
            .AddScoped<IListEnricher, EmployeesEnricher>()
            .AddScoped<GenreEnricher>()
            .AddScoped<IEnricher, GenreEnricher>()
            .AddScoped<GenresEnricher>()
            .AddScoped<IListEnricher, GenresEnricher>()
            .AddScoped<InvoiceEnricher>()
            .AddScoped<IEnricher, InvoiceEnricher>()
            .AddScoped<InvoicesEnricher>()
            .AddScoped<IListEnricher, InvoicesEnricher>()
            .AddScoped<InvoiceLineEnricher>()
            .AddScoped<IEnricher, InvoiceLineEnricher>()
            .AddScoped<InvoiceLinesEnricher>()
            .AddScoped<IListEnricher, InvoiceLinesEnricher>()
            .AddScoped<MediaTypeEnricher>()
            .AddScoped<IEnricher, MediaTypeEnricher>()
            .AddScoped<MediaTypesEnricher>()
            .AddScoped<IListEnricher, MediaTypesEnricher>()
            .AddScoped<PlaylistEnricher>()
            .AddScoped<IEnricher, PlaylistEnricher>()
            .AddScoped<PlaylistsEnricher>()
            .AddScoped<IListEnricher, PlaylistsEnricher>()
            .AddScoped<TrackEnricher>()
            .AddScoped<IEnricher, TrackEnricher>()
            .AddScoped<TracksEnricher>()
            .AddScoped<IListEnricher, TracksEnricher>();

        services.AddScoped<RepresentationEnricher>();
    }
}

public class ConfigureSwaggerOptions : IConfigureNamedOptions<SwaggerGenOptions>
{
    private readonly IApiVersionDescriptionProvider provider;

    public ConfigureSwaggerOptions(IApiVersionDescriptionProvider provider)
    {
        this.provider = provider;
    }

    public void Configure(SwaggerGenOptions options)
    {
        // add swagger document for every API version discovered
        foreach (var description in provider.ApiVersionDescriptions)
        {
            options.SwaggerDoc(
                description.GroupName,
                CreateVersionInfo(description));
            options.EnableAnnotations();
        }
    }

    public void Configure(string? name, SwaggerGenOptions options)
    {
        Configure(options);
    }

    private OpenApiInfo CreateVersionInfo(ApiVersionDescription description)
    {
        var info = new OpenApiInfo()
        {
            Version = "v1",
            Title = "Chinook Music Store API",
            Description = "A simple example ASP.NET Core Web API",
            TermsOfService = new Uri("https://example.com/terms"),
            Contact = new OpenApiContact
            {
                Name = "Chris Woodruff",
                Email = string.Empty,
                Url = new Uri("https://chriswoodruff.com")
            },
            License = new OpenApiLicense
            {
                Name = "Use under MIT",
                Url = new Uri("https://opensource.org/licenses/MIT")
            }
        };

        if (description.IsDeprecated)
        {
            info.Description += " This API version has been deprecated.";
        }

        return info;
    }
}