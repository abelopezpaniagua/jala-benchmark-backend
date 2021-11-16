using Domain.Abstractions;
using Services.Implementations;
using Microsoft.Extensions.DependencyInjection;

namespace Services.DependencyInjection
{
    public static class ServiceConfig
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddScoped<IProductService, ProductService>();

            return services;
        }
    }
}
