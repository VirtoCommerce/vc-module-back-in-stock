using System.Collections.Generic;
using VirtoCommerce.Platform.Core.Settings;

namespace VirtoCommerce.BackInStock.Core;

public static class ModuleConstants
{
    public static class Security
    {
        public static class Permissions
        {
            public const string Access = "BackInStock:access";
            public const string Create = "BackInStock:create";
            public const string Read = "BackInStock:read";
            public const string Update = "BackInStock:update";
            public const string Delete = "BackInStock:delete";

            public static string[] AllPermissions { get; } =
            {
                Access,
                Create,
                Read,
                Update,
                Delete,
            };
        }
    }

    public static class Settings
    {
        public static class General
        {
            public static SettingDescriptor BackInStockEnabled { get; } = new()
            {
                Name = "BackInStock.BackInStockEnabled",
                GroupName = "BackInStock|General",
                ValueType = SettingValueType.Boolean,
                DefaultValue = false,
            };

            public static SettingDescriptor BackInStockPassword { get; } = new()
            {
                Name = "BackInStock.BackInStockPassword",
                GroupName = "BackInStock|Advanced",
                ValueType = SettingValueType.SecureString,
                DefaultValue = "qwerty",
            };

            public static IEnumerable<SettingDescriptor> AllGeneralSettings
            {
                get
                {
                    yield return BackInStockEnabled;
                    yield return BackInStockPassword;
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
