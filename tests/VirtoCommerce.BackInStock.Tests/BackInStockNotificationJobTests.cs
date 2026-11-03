using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using VirtoCommerce.BackInStock.Core.Services;
using VirtoCommerce.BackInStock.Data.BackgroundJobs;
using VirtoCommerce.BackInStock.Data.Handlers;
using VirtoCommerce.CatalogModule.Core.Services;
using VirtoCommerce.CustomerModule.Core.Services;
using VirtoCommerce.InventoryModule.Core.Events;
using VirtoCommerce.InventoryModule.Core.Model;
using VirtoCommerce.NotificationsModule.Core.Services;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Events;
using VirtoCommerce.Platform.Core.Jobs;
using VirtoCommerce.Platform.Core.Settings;
using VirtoCommerce.StoreModule.Core.Services;
using Xunit;

namespace VirtoCommerce.BackInStock.Tests;

// Any other test class that enqueues through the static BackgroundJob facade must join this collection:
// the facade has no reset API (Initialize rejects null), so Dispose leaves a DISPOSED provider behind in
// the static, and a class racing this one would see ObjectDisposedException from it.
[Trait("Category", "Unit")]
[Collection(nameof(BackInStockNotificationJobTests))]
public class BackInStockNotificationJobTests
{
    [Fact]
    public async Task InventoryChanged_ProductBackInStock_EnqueuesNotificationJob()
    {
        //Arrange
        using var capture = new EnqueueCapture();
        var handler = new InventoryChangedEventHandler(CreateJob(CreateJobHandlerMock().Object));

        var message = new InventoryChangedEvent(
        [
            new GenericChangedEntry<InventoryInfo>(
                new InventoryInfo { ProductId = "product1", InStockQuantity = 5 },
                new InventoryInfo { ProductId = "product1", InStockQuantity = 0 },
                EntryState.Modified),
            new GenericChangedEntry<InventoryInfo>(
                new InventoryInfo { ProductId = "product1", InStockQuantity = 3 },
                new InventoryInfo { ProductId = "product1", InStockQuantity = 0 },
                EntryState.Modified),
            new GenericChangedEntry<InventoryInfo>(
                new InventoryInfo { ProductId = "product2", InStockQuantity = 1 },
                null,
                EntryState.Added),
        ]);

        //Act
        await handler.Handle(message);

        //Assert
        var payload = Assert.IsType<BackInStockNotificationJobPayload>(capture.Payload);
        Assert.Equal(1, capture.EnqueueCount);
        Assert.Equal(new[] { "product1", "product2" }, payload.ProductIds);
    }

    [Fact]
    public async Task InventoryChanged_ProductStillInStock_EnqueuesNothing()
    {
        //Arrange
        using var capture = new EnqueueCapture();
        var handler = new InventoryChangedEventHandler(CreateJob(CreateJobHandlerMock().Object));

        var message = new InventoryChangedEvent(
        [
            new GenericChangedEntry<InventoryInfo>(
                new InventoryInfo { ProductId = "product1", InStockQuantity = 5 },
                new InventoryInfo { ProductId = "product1", InStockQuantity = 3 },
                EntryState.Modified),
            new GenericChangedEntry<InventoryInfo>(
                new InventoryInfo { ProductId = "product2", InStockQuantity = 0 },
                null,
                EntryState.Added),
        ]);

        //Act
        await handler.Handle(message);

        //Assert
        Assert.Equal(0, capture.EnqueueCount);
    }

    [Fact]
    public async Task Run_LegacyJob_ForwardsToJobHandler()
    {
        //Arrange
        var jobHandlerMock = CreateJobHandlerMock();
        var job = CreateJob(jobHandlerMock.Object);
        IList<string> productIds = ["product1"];

        //Act
#pragma warning disable VC0015
        await job.Run(productIds);
#pragma warning restore VC0015

        //Assert
        jobHandlerMock.Verify(x => x.Execute(
                It.Is<BackInStockNotificationJobPayload>(payload => payload.ProductIds == productIds),
                It.IsAny<IJobExecutionContext>(),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    private static BackInStockNotificationJob CreateJob(BackInStockNotificationJobHandler jobHandler)
    {
        return new BackInStockNotificationJob(jobHandler, Mock.Of<ILogger<BackInStockNotificationJob>>());
    }

    private static Mock<BackInStockNotificationJobHandler> CreateJobHandlerMock()
    {
        var notificationBuilder = new BackInStockNotificationBuilder(
            Mock.Of<INotificationSearchService>(),
            Mock.Of<IStoreService>(),
            Mock.Of<IItemService>(),
            () => null,
            Mock.Of<IMemberService>());

        var jobHandlerMock = new Mock<BackInStockNotificationJobHandler>(
            Mock.Of<ISettingsManager>(),
            notificationBuilder,
            Mock.Of<INotificationSender>(),
            Mock.Of<IBackInStockSubscriptionService>(),
            Mock.Of<IBackInStockSubscriptionSearchService>(),
            Mock.Of<ILogger<BackInStockNotificationJobHandler>>());

        jobHandlerMock
            .Setup(x => x.Execute(It.IsAny<BackInStockNotificationJobPayload>(), It.IsAny<IJobExecutionContext>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        return jobHandlerMock;
    }

    // Captures what the job enqueued through the static BackgroundJob facade. IBackgroundJob is registered
    // Scoped here exactly as the engine module registers it, so this also proves the facade's per-call scope
    // resolves it - the job itself is a singleton and must never hold it.
    private sealed class EnqueueCapture : IDisposable
    {
        private readonly ServiceProvider _provider;

        public EnqueueCapture()
        {
            BackgroundJobMock
                .Setup(x => x.Enqueue<BackInStockNotificationJobHandler>(It.IsAny<object>(), It.IsAny<EnqueueOptions>(), It.IsAny<CancellationToken>()))
                .Callback<object, EnqueueOptions, CancellationToken>((payload, _, _) =>
                {
                    Payload = payload;
                    EnqueueCount++;
                })
                .ReturnsAsync("job-id");

            var services = new ServiceCollection();
            services.AddScoped(_ => BackgroundJobMock.Object);
            _provider = services.BuildServiceProvider(validateScopes: true);

            BackgroundJob.Initialize(_provider);
        }

        public Mock<IBackgroundJob> BackgroundJobMock { get; } = new();

        public object Payload { get; private set; }

        public int EnqueueCount { get; private set; }

        public void Dispose()
        {
            _provider.Dispose();
        }
    }
}
