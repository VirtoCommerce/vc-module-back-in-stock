using System.Collections.Generic;
using VirtoCommerce.BackInStockModule.Core.Models;
using VirtoCommerce.Platform.Core.Events;

namespace VirtoCommerce.BackInStockModule.Core.Events;

public class BackInStockSubscriptionChangedEvent(
    IEnumerable<GenericChangedEntry<BackInStockSubscription>> changedEntries)
    : GenericChangedEntryEvent<BackInStockSubscription>(changedEntries);
