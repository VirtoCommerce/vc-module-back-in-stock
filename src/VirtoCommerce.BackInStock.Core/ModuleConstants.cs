using System.Collections.Generic;
using VirtoCommerce.Platform.Core.Settings;

namespace VirtoCommerce.BackInStock.Core
{
    public static class ModuleConstants
    {
        public static class Security
        {
            public static class Permissions
            {
                public const string BackInStockSubscriptionRead = "backInStock:subscriptionRead";
                public const string BackInStockSubscriptionUpdate = "backInStock:subscriptionUpdate";
                public const string BackInStockSubscriptionDelete = "backInStock:subscriptionDelete";

                public static string[] AllPermissions =
                {
                    BackInStockSubscriptionRead, BackInStockSubscriptionUpdate, BackInStockSubscriptionDelete
                };
            }
        }

        public static class Settings
        {
            public static class General
            {
                public static readonly SettingDescriptor BackInStockEnabled = new SettingDescriptor
                {
                    Name = "BackInStock.BackInStockEnabled",
                    GroupName = "Store|Back In Stock",
                    ValueType = SettingValueType.Boolean,
                    DefaultValue = false,
                    IsPublic = true,
                };

                public static readonly SettingDescriptor BackInStockEnabledForAnonymous = new SettingDescriptor
                {
                    Name = "BackInStock.BackInStockEnabledForAnonymous",
                    GroupName = "Store|Back In Stock",
                    ValueType = SettingValueType.Boolean,
                    DefaultValue = false,
                    IsPublic = true,
                };

                
                public static SettingDescriptor SubscriptionsJobBatchSize { get; } = new()
                {
                    Name = "BackInStock.BatchSize",
                    GroupName = "Back In Stock|General",
                    ValueType = SettingValueType.Integer,
                    DefaultValue = 1000,
                };

                public static IEnumerable<SettingDescriptor> AllSettings
                {
                    get
                    {
                        yield return BackInStockEnabled;
                        yield return BackInStockEnabledForAnonymous;
                        yield return SubscriptionsJobBatchSize;
                    }
                }
            }

            public static IEnumerable<SettingDescriptor> StoreSettings
            {
                get
                {
                    yield return General.BackInStockEnabled;
                    yield return General.BackInStockEnabledForAnonymous;
                }
            }

            /*public static IEnumerable<SettingDescriptor> JobSettings
            {
                get
                {

                }
            }*/

            public static IEnumerable<SettingDescriptor> AllSettings
            {
                get
                {
                    return General.AllSettings;
                }
            }
        }
    }
}
