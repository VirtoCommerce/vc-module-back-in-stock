// Call this to register your module to main application
var moduleName = 'VirtoCommerce.BackInStock';

if (AppDependencies !== undefined) {
    AppDependencies.push(moduleName);
}

angular.module(moduleName, [])
    .config(['$stateProvider',
        function ($stateProvider) {
            $stateProvider
                .state('workspace.BackInStock', {
                    url: '/BackInStock',
                    templateUrl: '$(Platform)/Scripts/common/templates/home.tpl.html',
                    controller: [
                        'platformWebApp.bladeNavigationService',
                        function (bladeNavigationService) {
                            var newBlade = {
                                id: 'blade1',
                                controller: 'BackInStockModule.backInStockController',
                                template: 'Modules/$(VirtoCommerce.BackInStock)/Scripts/blades/BackInStockModule.html',
                                isClosingDisabled: true,
                            }
                            bladeNavigationService.showBlade(newBlade);
                        }
                    ]
                });
        }
    ])
    .run(['platformWebApp.mainMenuService', 'platformWebApp.widgetService', '$state',
        'platformWebApp.authService',
        function (mainMenuService, widgetService, $state, authService) {
            //Register module in main menu
            var menuItem = {
                path: 'browse/BackInStock',
                icon: 'fa fa-cube',
                title: 'Back In Stock',
                priority: 100,
                action: function () {
                    $state.go('workspace.BackInStock');
                },
                permission: 'BackInStock:access',
            };
            mainMenuService.addMenuItem(menuItem);
        }
    ]);
