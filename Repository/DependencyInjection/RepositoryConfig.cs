using Domain.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Repository.Implementations;

namespace Repository.DependencyInjection
{
    public static class RepositoryConfig
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IProductRepository, ProductRepository>();

            return services;
        }
    }
}
