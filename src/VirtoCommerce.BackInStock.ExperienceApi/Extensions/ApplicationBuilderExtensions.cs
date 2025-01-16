using Microsoft.AspNetCore.Builder;
using VirtoCommerce.Xapi.Core.Extensions;

namespace VirtoCommerce.BackInStock.ExperienceApi.Extensions;

public static class ApplicationBuilderExtensions
{
    public static void UseExperienceApi(this IApplicationBuilder applicationBuilder)
    {
        applicationBuilder.UseScopedSchema<AssemblyMarker>("back-in-stock");
    }
}
