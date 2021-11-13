using BragaPets.Domain.Contracts.Repositories;
using BragaPets.Domain.Contracts.Services;
using BragaPets.Domain.Notifications;
using BragaPets.Domain.Services;
using BragaPets.Infra.Data.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace BragaPets.Infra.CrossCutting.IoC
{
    public static class DependencyInjectionExtension
    {
        public static void ConfigureIoC(
            this IServiceCollection serviceCollection, IConfiguration configuration)
        {
            serviceCollection
                .RegisterRepositories(configuration)
                .RegisterServices()
                .RedisConfiguration();
        }

        private static IServiceCollection RegisterRepositories(
            this IServiceCollection serviceCollection, IConfiguration configuration)
        {
            var connectionString = configuration.GetSection("SqlServer").Value;
            
            serviceCollection.AddScoped<IUnitOfWork>(_ => new UnitOfWork(connectionString));
            serviceCollection.AddScoped<IUserRepository, UserRepository>();

            return serviceCollection;
        }
        
        private static IServiceCollection RegisterServices(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<IUserService, UserService>();
            serviceCollection.AddScoped<NotificationContext>();

            return serviceCollection;
        }

        private static void RedisConfiguration(this IServiceCollection serviceCollection)
        {
            var config = new ConfigurationOptions
            {
                EndPoints =
                {
                    { "localhost", 6379 },
                },
                Password = "redis",
            };

            serviceCollection.AddStackExchangeRedisCache(options =>
            {
                options.ConfigurationOptions = config;
                options.InstanceName = "BragaPets_";
            });

        }
    }
}