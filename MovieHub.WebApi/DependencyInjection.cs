using System.Text.Json.Serialization;
using MovieHub.WebApi.Filters;

namespace MovieHub.WebApi;

public static class DependencyInjection
{
    public static IServiceCollection AddWebServices(this IServiceCollection services)
    {
        services.AddControllers(options =>
            {
                options.Filters.Add(new ApiExceptionFilter());
            })
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
                options.JsonSerializerOptions.WriteIndented = true;
            });
        
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();   

        return services;
    }
}