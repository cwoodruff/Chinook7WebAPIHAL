using AutoMapper.EquivalencyExpression;
using Chinook.API.Profiles;
using Chinook.Domain.Repositories;
using Chinook.Domain.Supervisor;
using Chinook.Data.Repositories;
using Chinook.Domain.ApiModels;
using Chinook.Domain.Enrichers;
using Chinook.Domain.Helpers;
using Chinook.Domain.Validation;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.HttpLogging;

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

    public static void ConfigureValidators(this IServiceCollection services)
    {
        services.AddFluentValidation()
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

    public static void AddCaching(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddResponseCaching();
    }
    
    public static void AddHypermedia(this IServiceCollection services)
    {
        services.AddHttpContextAccessor();
        
        services.AddScoped<AlbumEnricher>()
            .AddScoped<IEnricher, AlbumEnricher>()
            .AddScoped<AlbumsEnricher>()
            .AddScoped<IEnricher, AlbumsEnricher>()
            .AddScoped<ArtistEnricher>()
            .AddScoped<IEnricher, ArtistEnricher>()
            .AddScoped<ArtistsEnricher>()
            .AddScoped<IEnricher, ArtistsEnricher>()
            .AddScoped<CustomerEnricher>()
            .AddScoped<IEnricher, CustomerEnricher>()
            .AddScoped<CustomersEnricher>()
            .AddScoped<IEnricher, CustomersEnricher>()
            .AddScoped<EmployeeEnricher>()
            .AddScoped<IEnricher, EmployeeEnricher>()
            .AddScoped<EmployeesEnricher>()
            .AddScoped<IEnricher, EmployeesEnricher>()
            .AddScoped<GenreEnricher>()
            .AddScoped<IEnricher, GenreEnricher>()
            .AddScoped<GenresEnricher>()
            .AddScoped<IEnricher, GenresEnricher>()
            .AddScoped<InvoiceEnricher>()
            .AddScoped<IEnricher, InvoiceEnricher>()
            .AddScoped<InvoicesEnricher>()
            .AddScoped<IEnricher, InvoicesEnricher>()
            .AddScoped<InvoiceLineEnricher>()
            .AddScoped<IEnricher, InvoiceLineEnricher>()
            .AddScoped<InvoiceLinesEnricher>()
            .AddScoped<IEnricher, InvoiceLinesEnricher>()
            .AddScoped<MediaTypeEnricher>()
            .AddScoped<IEnricher, MediaTypeEnricher>()
            .AddScoped<MediaTypesEnricher>()
            .AddScoped<IEnricher, MediaTypesEnricher>()
            .AddScoped<PlaylistEnricher>()
            .AddScoped<IEnricher, PlaylistEnricher>()
            .AddScoped<PlaylistsEnricher>()
            .AddScoped<IEnricher, PlaylistsEnricher>()
            .AddScoped<TrackEnricher>()
            .AddScoped<IEnricher, TrackEnricher>()
            .AddScoped<TracksEnricher>()
            .AddScoped<IEnricher, TracksEnricher>();

        services.AddScoped<RepresentationEnricher>();
    }

    public static void AddAutoMapperConfig(this IServiceCollection services)
    {
        services.AddAutoMapper((serviceProvider, automapper) =>
        {
            automapper.AddCollectionMappers();
        }, typeof(MapperConfig));
    }
}