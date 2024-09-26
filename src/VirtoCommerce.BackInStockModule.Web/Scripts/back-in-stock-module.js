// Call this to register your module to main application
const moduleName = 'VirtoCommerce.BackInStockModule';

if (AppDependencies !== undefined) {
    AppDependencies.push(moduleName);
}

angular.module(moduleName, [])
    .run(['$state',
        'platformWebApp.widgetService',
        'platformWebApp.authService',
        function ($state, widgetService, authService) {
            let customerBackInStockSubscriptionsWidget = {
                controller: 'VirtoCommerce.BackInStockModule.backInStockSubscriptionsWidgetController',
                template: 'Modules/$(VirtoCommerce.BackInStockModule)/Scripts/widgets/back-in-stock-widget.tpl.html',
                isVisible: true,
            };

            widgetService.registerWidget(customerBackInStockSubscriptionsWidget, 'customerDetail1');

            function checkPermissionToReadPrices() {
                return authService.checkPermission('BackInStockModule:access');
            }
        }
    ]);
