using System;
using System.Collections.Generic;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.BackInStockModule.Core.Models;

public class BackInStockSubscriptionSearchCriteria : SearchCriteriaBase
{
    public string UserId { get; set; }

    public string StoreId { get; set; }

    public IList<string> ProductIds { get; set; }

    public bool? IsActive { get; set; }

    public DateTime? StartTriggeredDate { get; set; }

    public DateTime? EndTriggeredDate { get; set; }
}
