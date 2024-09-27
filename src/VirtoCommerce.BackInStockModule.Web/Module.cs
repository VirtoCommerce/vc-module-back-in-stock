using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VirtoCommerce.BackInStockModule.Core;
using VirtoCommerce.BackInStockModule.Core.BackgroundJobs;
using VirtoCommerce.BackInStockModule.Core.Notifications;
using VirtoCommerce.BackInStockModule.Core.Services;
using VirtoCommerce.BackInStockModule.Data.BackgroundJobs;
using VirtoCommerce.BackInStockModule.Data.Handlers;
using VirtoCommerce.BackInStockModule.Data.MySql;
using VirtoCommerce.BackInStockModule.Data.PostgreSql;
using VirtoCommerce.BackInStockModule.Data.Repositories;
using VirtoCommerce.BackInStockModule.Data.Services;
using VirtoCommerce.BackInStockModule.Data.SqlServer;
using VirtoCommerce.BackInStockModule.ExperienceApi.Extensions;
using VirtoCommerce.InventoryModule.Core.Events;
using VirtoCommerce.NotificationsModule.Core.Services;
using VirtoCommerce.Platform.Core.Events;
using VirtoCommerce.Platform.Core.Modularity;
using VirtoCommerce.Platform.Core.Security;
using VirtoCommerce.Platform.Core.Settings;
using VirtoCommerce.Platform.Data.MySql.Extensions;
using VirtoCommerce.Platform.Data.PostgreSql.Extensions;
using VirtoCommerce.Platform.Data.SqlServer.Extensions;

namespace VirtoCommerce.BackInStockModule.Web;

public class Module : IModule, IHasConfiguration
{
    public ManifestModuleInfo ModuleInfo { get; set; }
    public IConfiguration Configuration { get; set; }

    public void Initialize(IServiceCollection serviceCollection)
    {
        serviceCollection.AddDbContext<BackInStockModuleDbContext>(options =>
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

        serviceCollection.AddTransient<IBackInStockSubscriptionRepository, BackInStockSubscriptionRepository>();
        serviceCollection.AddSingleton<Func<IBackInStockSubscriptionRepository>>(provider =>
            () => provider.CreateScope().ServiceProvider.GetRequiredService<IBackInStockSubscriptionRepository>());
        serviceCollection.AddTransient<IBackInStockSubscriptionService, BackInStockSubscriptionService>();
        serviceCollection
            .AddTransient<IBackInStockSubscriptionSearchService, BackInStockSubscriptionSearchService>();
        serviceCollection.AddSingleton<InventoryChangedEventHandler>();
        serviceCollection.AddSingleton<IBackInStockNotificationJobService, BackInStockNotificationJobService>();
        serviceCollection.AddExperienceApi();
    }

    public void PostInitialize(IApplicationBuilder appBuilder)
    {
        var serviceProvider = appBuilder.ApplicationServices;

        // Register settings
        var settingsRegistrar = serviceProvider.GetRequiredService<ISettingsRegistrar>();
        settingsRegistrar.RegisterSettings(ModuleConstants.Settings.AllSettings, ModuleInfo.Id);

        // Register permissions
        var permissionsRegistrar = serviceProvider.GetRequiredService<IPermissionsRegistrar>();
        permissionsRegistrar.RegisterPermissions(ModuleInfo.Id, "BackInStockModule", ModuleConstants.Security.Permissions.AllPermissions);

        var notificationRegistrar = appBuilder.ApplicationServices.GetService<INotificationRegistrar>();
        notificationRegistrar.RegisterNotification<BackInStockEmailNotification>();

        appBuilder.RegisterEventHandler<InventoryChangedEvent, InventoryChangedEventHandler>();

        // Apply migrations
        using var serviceScope = serviceProvider.CreateScope();
        using var dbContext = serviceScope.ServiceProvider.GetRequiredService<BackInStockModuleDbContext>();
        dbContext.Database.Migrate();
    }

    public void Uninstall()
    {
        // Nothing to do here
    }
}
