using System.Collections.Generic;
using VirtoCommerce.Platform.Core.Settings;

namespace VirtoCommerce.BackInStock.Core;

public static class ModuleConstants
{
    public static class Security
    {
        public static class Permissions
        {
            public const string Access = "BackInStock:subscription:access";
            public const string Create = "BackInStock:subscription:create";
            public const string Read = "BackInStock:subscription:read";
            public const string Update = "BackInStock:subscription:update";
            public const string Delete = "BackInStock:subscription:delete";

            public static string[] AllPermissions { get; } =
            [
                Access,
                Create,
                Read,
                Update,
                Delete,
            ];
        }
    }

    public static class Settings
    {
        public static class General
        {
            public static SettingDescriptor BackInStockEnabled { get; } = new()
            {
                Name = "BackInStock.BackInStockEnabled",
                GroupName = "Back In Stock|General",
                ValueType = SettingValueType.Boolean,
                DefaultValue = false,
                IsPublic = true,
            };

            public static SettingDescriptor BackInStockEnabledForAnonymous { get; } = new()
            {
                Name = "BackInStock.BackInStockEnabledForAnonymous",
                GroupName = "Back In Stock|General",
                ValueType = SettingValueType.Boolean,
                DefaultValue = false,
                IsPublic = true,
            };

            public static SettingDescriptor SubscriptionsJobBatchSize { get; } = new()
            {
                Name = "BackInStock.BatchSize",
                DisplayName = "Notifications to schedule batch size",
                GroupName = "Back In Stock|General",
                ValueType = SettingValueType.Integer,
                DefaultValue = 1000,
            };

            public static IEnumerable<SettingDescriptor> AllGeneralSettings
            {
                get
                {
                    yield return BackInStockEnabled;
                    yield return BackInStockEnabledForAnonymous;
                    yield return SubscriptionsJobBatchSize;
                }
            }
        }

        public static IEnumerable<SettingDescriptor> AllSettings
        {
            get
            {
                return General.AllGeneralSettings;
            }
        }
    }
}
