using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Identity;
using VirtoCommerce.BackInStock.Core.Models;
using VirtoCommerce.BackInStock.Core.Services;
using VirtoCommerce.CatalogModule.Core.Services;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Security;
using VirtoCommerce.StoreModule.Core.Services;

namespace VirtoCommerce.BackInStock.ExperienceApi.Commands;

public class ActivateBackInStockSubscriptionCommandHandler(
    IStoreService storeService,
    IItemService itemService,
    Func<UserManager<ApplicationUser>> userManagerFactory,
    IBackInStockSubscriptionService crudService,
    IBackInStockSubscriptionSearchService searchService)
    : IRequestHandler<ActivateBackInStockSubscriptionCommand, BackInStockSubscription>
{

    public async Task<BackInStockSubscription> Handle(ActivateBackInStockSubscriptionCommand request, CancellationToken cancellationToken)
    {
        var store = await storeService.GetByIdAsync(request.StoreId);
        if (store == null)
        {
            return null;
        }

        var product = await itemService.GetByIdAsync(request.ProductId);
        if (product == null)
        {
            return null;
        }

        using var userManager = userManagerFactory();
        var user = await userManager.FindByIdAsync(request.UserId);
        if (user == null)
        {
            return null;
        }

        var searchCriteria = AbstractTypeFactory<BackInStockSubscriptionSearchCriteria>.TryCreateInstance();
        searchCriteria.StoreId = request.StoreId;
        searchCriteria.ProductIds = [request.ProductId];
        searchCriteria.UserId = request.UserId;
        searchCriteria.Take = 1;

        var searchResult = await searchService.SearchAsync(searchCriteria);

        var subscription = searchResult.Results.FirstOrDefault();
        if (subscription == null)
        {
            subscription = AbstractTypeFactory<BackInStockSubscription>.TryCreateInstance();
        }

        subscription.StoreId = store.Id;
        subscription.ProductId = product.Id;
        subscription.ProductCode = product.Code;
        subscription.ProductName = product.Name;
        subscription.UserId = user.Id;
        subscription.MemberId = user.MemberId;
        subscription.IsActive = true;

        await crudService.SaveChangesAsync([subscription]);

        return subscription;
    }
}
