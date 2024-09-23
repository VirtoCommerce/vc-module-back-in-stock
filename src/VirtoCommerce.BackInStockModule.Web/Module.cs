using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VirtoCommerce.BackInStockModule.Core;
using VirtoCommerce.BackInStockModule.Core.Notifications;
using VirtoCommerce.BackInStockModule.Core.Services;
using VirtoCommerce.BackInStockModule.Data.Handlers;
using VirtoCommerce.BackInStockModule.Data.MySql;
using VirtoCommerce.BackInStockModule.Data.PostgreSql;
using VirtoCommerce.BackInStockModule.Data.Repositories;
using VirtoCommerce.BackInStockModule.Data.Services;
using VirtoCommerce.BackInStockModule.Data.SqlServer;
using VirtoCommerce.BackInStockModule.ExperienceApi.Extensions;
using VirtoCommerce.InventoryModule.Core.Events;
using VirtoCommerce.NotificationsModule.Core.Services;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Events;
using VirtoCommerce.Platform.Core.Modularity;
using VirtoCommerce.Platform.Core.Security;
using VirtoCommerce.Platform.Core.Settings;
using VirtoCommerce.Platform.Data.Extensions;
using VirtoCommerce.StoreModule.Core.Model;
using BackInStockSettings = VirtoCommerce.BackInStockModule.Core.ModuleConstants.Settings;

namespace VirtoCommerce.BackInStockModule.Web
{
    public class Module : IModule, IHasConfiguration
    {
        private IApplicationBuilder _applicationBuilder;

        public ManifestModuleInfo ModuleInfo { get; set; }
        public IConfiguration Configuration { get; set; }

        public void Initialize(IServiceCollection serviceCollection)
        {
            serviceCollection.AddDbContext<BackInStockDbContext>(options =>
            {
                var databaseProvider = Configuration.GetValue("DatabaseProvider", "SqlServer");
                var connectionString = Configuration.GetConnectionString(ModuleInfo.Id) ??
                                       Configuration.GetConnectionString("VirtoCommerce");

                switch (databaseProvider)
                {
                    case "MySql":
                        options.UseMySqlDatabase(connectionString);
                        break;
                    case "PostgreSql":
                        options.UsePostgreSqlDatabase(connectionString);
                        break;
                    default:
                        options.UseSqlServerDatabase(connectionString);
                        break;
                }
            });

            serviceCollection.AddTransient<IBackInStockSubscriptionRepository, BackInStockSubscriptionRepository>();
            serviceCollection.AddSingleton<Func<IBackInStockSubscriptionRepository>>(provider =>
                () => provider.CreateScope().ServiceProvider.GetRequiredService<IBackInStockSubscriptionRepository>());

            serviceCollection.AddTransient<IBackInStockSubscriptionService, BackInStockSubscriptionService>();
            serviceCollection
                .AddTransient<IBackInStockSubscriptionSearchService, BackInStockSubscriptionSearchService>();
            serviceCollection.AddTransient<InventoryChangedEventHandler>();

            // GraphQL
            serviceCollection.AddExperienceApi();
        }

        public void PostInitialize(IApplicationBuilder appBuilder)
        {
            _applicationBuilder = appBuilder;

            var settingsManager = appBuilder.ApplicationServices.GetRequiredService<ISettingsManager>();

            var settingsRegistrar = appBuilder.ApplicationServices.GetRequiredService<ISettingsRegistrar>();
            settingsRegistrar.RegisterSettings(BackInStockSettings.AllSettings, ModuleInfo.Id);

            settingsRegistrar.RegisterSettingsForType(BackInStockSettings.StoreSettings, nameof(Store));

            var permissionsRegistrar = appBuilder.ApplicationServices.GetRequiredService<IPermissionsRegistrar>();
            permissionsRegistrar.RegisterPermissions(ModuleInfo.Id,
                "BackInStock",
                ModuleConstants.Security.Permissions.AllPermissions);

            var notificationRegistrar = appBuilder.ApplicationServices.GetService<INotificationRegistrar>();
            notificationRegistrar.RegisterNotification<BackInStockEmailNotification>();

            appBuilder.RegisterEventHandler<InventoryChangedEvent, InventoryChangedEventHandler>();

            /*var recurringJobService = appBuilder.ApplicationServices.GetService<IRecurringJobService>();
            recurringJobService.WatchJobSetting(
                new SettingCronJobBuilder()
                    .SetEnablerSetting(BackInStockSettings.General.RequestReviewEnableJob)
                    .SetCronSetting(BackInStockSettings.General.RequestReviewCronJob)
                    .ToJob<BackInStockNotificationsSendingJob>(x => x.Process())
                    .Build());*/

            using var serviceScope = appBuilder.ApplicationServices.CreateScope();
            var databaseProvider = Configuration.GetValue("DatabaseProvider", "SqlServer");
            var dbContext = serviceScope.ServiceProvider.GetRequiredService<BackInStockDbContext>();
            if (databaseProvider == "SqlServer")
            {
                dbContext.Database.MigrateIfNotApplied(MigrationName.GetUpdateV2MigrationName(ModuleInfo.Id));
            }

            dbContext.Database.Migrate();
        }

        public void Uninstall()
        {
            // Nothing to do here
        }
    }
}
