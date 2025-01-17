using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VirtoCommerce.BackInStock.Core;
using VirtoCommerce.BackInStock.Core.BackgroundJobs;
using VirtoCommerce.BackInStock.Core.Notifications;
using VirtoCommerce.BackInStock.Core.Services;
using VirtoCommerce.BackInStock.Data.BackgroundJobs;
using VirtoCommerce.BackInStock.Data.Handlers;
using VirtoCommerce.BackInStock.Data.MySql;
using VirtoCommerce.BackInStock.Data.PostgreSql;
using VirtoCommerce.BackInStock.Data.Repositories;
using VirtoCommerce.BackInStock.Data.Services;
using VirtoCommerce.BackInStock.Data.SqlServer;
using VirtoCommerce.BackInStock.ExperienceApi.Extensions;
using VirtoCommerce.InventoryModule.Core.Events;
using VirtoCommerce.NotificationsModule.Core.Services;
using VirtoCommerce.Platform.Core.Events;
using VirtoCommerce.Platform.Core.Modularity;
using VirtoCommerce.Platform.Core.Security;
using VirtoCommerce.Platform.Core.Settings;
using VirtoCommerce.Platform.Data.MySql.Extensions;
using VirtoCommerce.Platform.Data.PostgreSql.Extensions;
using VirtoCommerce.Platform.Data.SqlServer.Extensions;
using VirtoCommerce.StoreModule.Core.Model;

namespace VirtoCommerce.BackInStock.Web;

public class Module : IModule, IHasConfiguration
{
    public ManifestModuleInfo ModuleInfo { get; set; }
    public IConfiguration Configuration { get; set; }

    public void Initialize(IServiceCollection serviceCollection)
    {
        serviceCollection.AddDbContext<BackInStockDbContext>(options =>
        {
            var databaseProvider = Configuration.GetValue("DatabaseProvider", "SqlServer");
            var connectionString = Configuration.GetConnectionString(ModuleInfo.Id) ?? Configuration.GetConnectionString("VirtoCommerce");

            switch (databaseProvider)
            {
                case "MySql":
                    options.UseMySqlDatabase(connectionString, typeof(MySqlDataAssemblyMarker), Configuration);
                    break;
                case "PostgreSql":
                    options.UsePostgreSqlDatabase(connectionString, typeof(PostgreSqlDataAssemblyMarker), Configuration);
                    break;
                default:
                    options.UseSqlServerDatabase(connectionString, typeof(SqlServerDataAssemblyMarker), Configuration);
                    break;
            }
        });

        serviceCollection.AddTransient<IBackInStockRepository, BackInStockRepository>();
        serviceCollection.AddSingleton<Func<IBackInStockRepository>>(provider => () => provider.CreateScope().ServiceProvider.GetRequiredService<IBackInStockRepository>());

        serviceCollection.AddTransient<IBackInStockSubscriptionService, BackInStockSubscriptionService>();
        serviceCollection.AddTransient<IBackInStockSubscriptionSearchService, BackInStockSubscriptionSearchService>();

        serviceCollection.AddSingleton<InventoryChangedEventHandler>();
        serviceCollection.AddSingleton<IBackInStockNotificationJob, BackInStockNotificationJob>();

        serviceCollection.AddExperienceApi();
    }

    public void PostInitialize(IApplicationBuilder appBuilder)
    {
        var serviceProvider = appBuilder.ApplicationServices;

        // Register settings
        var settingsRegistrar = serviceProvider.GetRequiredService<ISettingsRegistrar>();
        settingsRegistrar.RegisterSettings(ModuleConstants.Settings.AllSettings, ModuleInfo.Id);

        // Register store settings
        settingsRegistrar.RegisterSettingsForType(ModuleConstants.Settings.StoreSettings, nameof(Store));

        // Register permissions
        var permissionsRegistrar = serviceProvider.GetRequiredService<IPermissionsRegistrar>();
        permissionsRegistrar.RegisterPermissions(ModuleInfo.Id, "BackInStock", ModuleConstants.Security.Permissions.AllPermissions);

        // Register notifications
        var notificationRegistrar = appBuilder.ApplicationServices.GetService<INotificationRegistrar>();
        notificationRegistrar.RegisterNotification<BackInStockEmailNotification>();

        // Register event handlers
        appBuilder.RegisterEventHandler<InventoryChangedEvent, InventoryChangedEventHandler>();

        appBuilder.UseExperienceApi();

        // Apply migrations
        using var serviceScope = serviceProvider.CreateScope();
        using var dbContext = serviceScope.ServiceProvider.GetRequiredService<BackInStockDbContext>();
        dbContext.Database.Migrate();
    }

    public void Uninstall()
    {
        // Nothing to do here
    }
}
