// Call this to register your module to main application
const moduleName = 'VirtoCommerce.BackInStock';

if (AppDependencies !== undefined) {
    AppDependencies.push(moduleName);
}

angular.module(moduleName, [])
    .run(['$state',
        'platformWebApp.widgetService',
        'platformWebApp.authService',
        function ($state, widgetService, authService) {
            let customerBackInStockSubscriptionsWidget = {
                controller: 'VirtoCommerce.BackInStock.backInStockSubscriptionsWidgetController',
                template: 'Modules/$(VirtoCommerce.BackInStock)/Scripts/widgets/back-in-stock-widget.html',
                isVisible: true,
            };

            widgetService.registerWidget(customerBackInStockSubscriptionsWidget, 'customerDetail1');

            function checkPermissionToReadPrices() {
                return authService.checkPermission('BackInStock:access');
            }
        }
    ]);
