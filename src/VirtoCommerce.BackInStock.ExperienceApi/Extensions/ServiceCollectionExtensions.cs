using GraphQL.Server;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using VirtoCommerce.ProfileExperienceApiModule.Data.Aggregates.Vendor;
using VirtoCommerce.Xapi.Core.Extensions;
using VirtoCommerce.Xapi.Core.Infrastructure;
using VirtoCommerce.Xapi.Core.Pipelines;
using VirtoCommerce.XCatalog.Core.Models;

namespace VirtoCommerce.BackInStock.ExperienceApi.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddExperienceApi(this IServiceCollection serviceCollection)
    {
        var assemblyMarker = typeof(AssemblyMarker);
        var graphQlBuilder = new CustomGraphQLBuilder(serviceCollection);
        graphQlBuilder.AddGraphTypes(assemblyMarker);
        serviceCollection.AddMediatR(assemblyMarker);
        serviceCollection.AddAutoMapper(assemblyMarker);
        serviceCollection.AddSchemaBuilders(assemblyMarker);
        return serviceCollection;
    }
}
