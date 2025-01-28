using GraphQL.MicrosoftDI;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using VirtoCommerce.BackInStock.ExperienceApi.Authorization;
using VirtoCommerce.Xapi.Core.Extensions;
using VirtoCommerce.Xapi.Core.Infrastructure;

namespace VirtoCommerce.BackInStock.ExperienceApi.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddExperienceApi(this IServiceCollection serviceCollection)
    {
        _ = new GraphQLBuilder(serviceCollection, builder =>
        {
            builder.AddSchema(serviceCollection, typeof(AssemblyMarker));
        });

        serviceCollection.AddSingleton<ScopedSchemaFactory<AssemblyMarker>>();
        serviceCollection.AddSingleton<IAuthorizationHandler, BackInStockAuthorizationHandler>();
    }
}
