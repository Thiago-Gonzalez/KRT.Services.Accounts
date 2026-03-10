using FluentValidation;
using KRT.Services.Accounts.Application.Commands;
using KRT.Services.Accounts.Application.MediatR;
using KRT.Services.Accounts.Application.Validators;
using Microsoft.Extensions.DependencyInjection;

namespace KRT.Services.Accounts.Application;

public static class ApplicationModule
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services
            .AddValidators()
            .AddMediatR()
            .AddHandlers();

        return services;
    }

    private static IServiceCollection AddHandlers(this IServiceCollection services)
    {
        var assembly = typeof(AddAccountCommand).Assembly;

        foreach (var type in assembly.GetTypes())
        {
            var interfaces = type.GetInterfaces()
                .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IRequestHandler<,>));

            foreach (var iface in interfaces)
            {
                services.AddScoped(iface, type);
            }
        }

        return services;
    }

    private static IServiceCollection AddMediatR(this IServiceCollection services)
    {
        services.AddScoped<IMediator, Mediator>();

        return services;
    }

    private static IServiceCollection AddValidators(this IServiceCollection services)
    {
        services.AddScoped<IValidator<AddAccountCommand>, AddAccountCommandValidator>();
        services.AddScoped<IValidator<UpdateAccountCommand>, UpdateAccountCommandValidator>();

        return services;
    }
}
