// Call this to register your module to main application
const moduleName = 'VirtoCommerce.BackInStock';

if (AppDependencies !== undefined) {
    AppDependencies.push(moduleName);
}

angular.module(moduleName, [])
    .run(['platformWebApp.widgetService',
        function (widgetService) {
            widgetService.registerWidget({
                controller: 'VirtoCommerce.BackInStock.subscriptionWidgetController',
                template: 'Modules/$(VirtoCommerce.BackInStock)/Scripts/widgets/back-in-stock-subscription-widget.html',
                isVisible: true,
            }, 'customerDetail1');
        }
    ]);
