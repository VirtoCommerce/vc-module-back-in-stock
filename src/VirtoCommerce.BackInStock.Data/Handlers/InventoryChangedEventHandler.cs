using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hangfire;
using VirtoCommerce.BackInStock.Core.Notifications;
using VirtoCommerce.BackInStock.Data.Models;
using VirtoCommerce.BackInStock.Data.Repositories;
using VirtoCommerce.CatalogModule.Core.Model.Search;
using VirtoCommerce.CatalogModule.Core.Search;
using VirtoCommerce.CustomerModule.Core.Services;
using VirtoCommerce.InventoryModule.Core.Events;
using VirtoCommerce.NotificationsModule.Core.Extensions;
using VirtoCommerce.NotificationsModule.Core.Services;
using VirtoCommerce.OrdersModule.Core.Model;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Events;
using VirtoCommerce.Platform.Core.Settings;
using VirtoCommerce.StoreModule.Core.Model;
using VirtoCommerce.StoreModule.Core.Services;
using BackInStockSettings = VirtoCommerce.BackInStock.Core.ModuleConstants.Settings.General;

namespace VirtoCommerce.BackInStock.Data.Handlers
{
    public class InventoryChangedEventHandler : IEventHandler<InventoryChangedEvent>
    {
        private readonly ISettingsManager _settingsManager;
        private readonly IProductSearchService _productSearchService;
        private readonly INotificationSearchService _notificationSearchService;
        private readonly INotificationSender _notificationSender;
        private readonly IMemberResolver _memberResolver;
        private readonly IStoreService _storeService;
        private readonly Func<IBackInStockSubscriptionRepository> _backInStockSubscriptionRepositoryFactory;

        public InventoryChangedEventHandler(
            ISettingsManager settingsManager,
            Func<IBackInStockSubscriptionRepository> backInStockSubscriptionRepositoryFactory,
            INotificationSearchService notificationSearchService,
            INotificationSender notificationSender,
            IMemberResolver memberResolver,
            IProductSearchService productSearchService,
            IStoreService storeService)
        {
            _settingsManager = settingsManager;
            _backInStockSubscriptionRepositoryFactory = backInStockSubscriptionRepositoryFactory;
            _notificationSearchService = notificationSearchService;
            _notificationSender = notificationSender;
            _memberResolver = memberResolver;
            _productSearchService = productSearchService;
            _storeService = storeService;
        }

        public async Task Handle(InventoryChangedEvent inventoryChangedEvent)
        {
            var inStockProducts = inventoryChangedEvent.ChangedEntries
                .Where(x => x.OldEntry.InStockQuantity == 0 && x.NewEntry.InStockQuantity > 0);
            using var repository = _backInStockSubscriptionRepositoryFactory();
            var activeProductSubscriptions =
                await repository.GetByIdsAsync(inStockProducts.Select(s => s.NewEntry.ProductId).ToList());

            foreach (var subscription in activeProductSubscriptions)
            {
                BackgroundJob.Enqueue(() => SendNotificationsAsync(subscription));
            }
        }

        protected virtual async Task SendNotificationsAsync(BackInStockSubscriptionEntity subscription)
        {
            var notification = await _notificationSearchService.GetNotificationAsync(
                    nameof(BackInStockEmailNotification),
                    new TenantIdentity(subscription.StoreId, nameof(Store)))
                as BackInStockEmailNotification;

            if (notification != null)
            {
                var customer = await _memberResolver.ResolveMemberByIdAsync(subscription.UserId);
                var store = (await _storeService.GetAsync(new List<string>() { subscription.StoreId }))
                    .FirstOrDefault();
                var product = await _productSearchService.SearchAsync(new ProductSearchCriteria()
                {
                    ObjectIds = new List<string>() { subscription.ProductId }
                });
                //notification.Item =
                notification.Customer = customer;
                //notification.RequestId = parameters.RequestId;
                //notification.LanguageCode = product.Results.FirstOrDefault().
                notification.From = store.EmailWithName;
                notification.To = customer.Emails.FirstOrDefault();

                await _notificationSender.ScheduleSendNotificationAsync(notification);
            }
        }

        private Task<int> GetBatchSize()
        {
            return _settingsManager.GetValueAsync<int>(BackInStockSettings.SubscriptionsJobBatchSize);
        }
    }
}
