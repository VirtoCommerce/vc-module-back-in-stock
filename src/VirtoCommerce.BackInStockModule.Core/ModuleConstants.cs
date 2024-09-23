using System.Collections.Generic;
using VirtoCommerce.Platform.Core.Settings;

namespace VirtoCommerce.BackInStockModule.Core;

public static class ModuleConstants
{
    public static class Security
    {
        public static class Permissions
        {
            public const string Access = "BackInStockModule:access";
            public const string Create = "BackInStockModule:create";
            public const string Read = "BackInStockModule:read";
            public const string Update = "BackInStockModule:update";
            public const string Delete = "BackInStockModule:delete";

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
            public static SettingDescriptor BackInStockModuleEnabled { get; } = new()
            {
                Name = "BackInStockModule.BackInStockModuleEnabled",
                GroupName = "BackInStockModule|General",
                ValueType = SettingValueType.Boolean,
                DefaultValue = false,
            };

            public static SettingDescriptor BackInStockModulePassword { get; } = new()
            {
                Name = "BackInStockModule.BackInStockModulePassword",
                GroupName = "BackInStockModule|Advanced",
                ValueType = SettingValueType.SecureString,
                DefaultValue = "qwerty",
            };

            public static IEnumerable<SettingDescriptor> AllGeneralSettings
            {
                get
                {
                    yield return BackInStockModuleEnabled;
                    yield return BackInStockModulePassword;
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
