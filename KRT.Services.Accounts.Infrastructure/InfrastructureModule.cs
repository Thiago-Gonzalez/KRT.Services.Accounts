using KRT.Services.Accounts.Core.Repositories;
using KRT.Services.Accounts.Infrastructure.MessageBus;
using KRT.Services.Accounts.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace KRT.Services.Accounts.Infrastructure;

public static class InfrastructureModule
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddDbContext(configuration)
            .AddRepositories()
            .AddRabbitMQ();

        return services;
    }

    public static IServiceCollection AddDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("KRTAccountsDb");

        services.AddDbContext<KRTAccountsDbContext>(options =>
            options.UseSqlServer(
                connectionString,
                sqlOptions => sqlOptions.EnableRetryOnFailure(
                    maxRetryCount: 5,
                    maxRetryDelay: TimeSpan.FromSeconds(10),
                    errorNumbersToAdd: null)));

        return services;
    }

    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IAccountRepository, IAccountRepository>();

        return services;
    }

    public static IServiceCollection AddRabbitMQ(this IServiceCollection services)
    {
        services.AddSingleton(sp => {
            var configuration = sp.GetService<IConfiguration>();
            var options = new RabbitMQOptions();

            configuration!.GetSection("RabbitMQ").Bind(options);

            return options;
        });

        services.AddSingleton(sp =>
        {
            var options = sp.GetRequiredService<IOptions<RabbitMQOptions>>().Value;

            var connectionFactory = new ConnectionFactory
            {
                HostName = options.Host,
                Port = options.Port,
                UserName = options.Username,
                Password = options.Password
            };

            var connection = connectionFactory.CreateConnectionAsync(options.ClientName).GetAwaiter().GetResult();

            return new ProducerConnection(connection);
        });

        services.AddSingleton<IMessageBusClient, RabbitMQClient>();
        services.AddTransient<IEventProcessor, EventProcessor>();

        return services;
    }
}
