using System.Collections.Generic;
using VirtoCommerce.BackInStock.Core.Models;
using VirtoCommerce.Platform.Core.Events;

namespace VirtoCommerce.BackInStock.Core.Events;

public class BackInStockSubscriptionChangingEvent(IEnumerable<GenericChangedEntry<BackInStockSubscription>> changedEntries)
    : GenericChangedEntryEvent<BackInStockSubscription>(changedEntries);
