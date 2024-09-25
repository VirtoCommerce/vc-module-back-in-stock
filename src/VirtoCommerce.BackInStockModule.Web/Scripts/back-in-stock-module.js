// Call this to register your module to main application
var moduleName = 'VirtoCommerce.BackInStockModule';

if (AppDependencies !== undefined) {
    AppDependencies.push(moduleName);
}

angular.module(moduleName, [])
    .config(['$stateProvider',
        function ($stateProvider) {
            $stateProvider
                .state('workspace.BackInStockModuleState', {
                    url: '/BackInStockModule',
                    templateUrl: '$(Platform)/Scripts/common/templates/home.tpl.html',
                    controller: [
                        'platformWebApp.bladeNavigationService',
                        function (bladeNavigationService) {
                            var newBlade = {
                                id: 'backInStockSubscriptionsList',
                                controller: 'VirtoCommerce.BackInStockModule.backInStockSubscriptionsController',
                                template: 'Modules/$(VirtoCommerce.BackInStockModule)/Scripts/blades/back-in-stock-module-subscriptions.html',
                                isClosingDisabled: true,
                            };
                            bladeNavigationService.showBlade(newBlade);
                        }
                    ]
                });
        }
    ])
    .run(['platformWebApp.mainMenuService',
        '$state',
        'platformWebApp.widgetService',
        'platformWebApp.authService',
        'VirtoCommerce.BackInStockModule.entityTypesResolverService',
        function (mainMenuService, $state, widgetService, authService, entityTypesResolverService) {
            //Register module in main menu
            var menuItem = {
                path: 'browse/BackInStockModule',
                icon: 'fa fa-cube',
                title: 'BackInStockModule',
                priority: 100,
                action: function () {
                    $state.go('workspace.BackInStockModuleState');
                },
                permission: 'BackInStockModule:access',
            };

            mainMenuService.addMenuItem(menuItem);

            var customerBackInStockSubscriptionsWidget = {
                controller: 'VirtoCommerce.BackInStockModule.backInStockSubscriptionsController',
                template: 'Modules/$(VirtoCommerce.BackInStockModule)/Scripts/blades/back-in-stock-module.html',
                isVisible: checkPermissionToReadPrices,
            };

            widgetService.registerWidget(customerBackInStockSubscriptionsWidget, 'customerBackInStockSubscriptionsWidget');

            function checkPermissionToReadPrices() {
                return authService.checkPermission('BackInStockModule:access');
            }

            //Product entityType resolver
            entityTypesResolverService.registerType({
                entityType: 'Product',
                description: 'VirtoCommerce.BackInStockModule.blades.product-detail.description',
                fullTypeName: 'VirtoCommerce.CatalogModule.Core.Model.CatalogProduct',
                icon: 'fas fa-shopping-bag',
                entityIdFieldName: 'itemId',
                detailBlade: {
                    controller: 'virtoCommerce.catalogModule.itemDetailController',
                    template: 'Modules/$(VirtoCommerce.Catalog)/Scripts/blades/item-detail.tpl.html'
                },
                getEntity: function (entityId, setEntityCallback) {
                    items.get({id: entityId, respGroup: 1}, (data) => {
                        setEntityCallback(data.name, data.imgSrc);
                    });
                },
                knownChildrenTypes: []
            });
        }
    ]);
