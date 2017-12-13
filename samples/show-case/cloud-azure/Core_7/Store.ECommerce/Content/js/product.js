(function () {

    angular.module('sample', [])
        .controller('ProductCtrl', function ProductCtrl($scope) {
            $scope.debug = false;
            $scope.errorMessage = null;

            $scope.products = [
              { id: 'videos', title: 'Videos and Presentations', description: 'Learn about SOA principles, systems design and the capabilities our platform brings to your system lifecycle.', selected: false },
              { id: 'training', title: 'Get some on-site training', description: 'The fastest way to get a large team up to speed', selected: false },
              { id: 'documentation', title: 'Community driven documentation', description: 'At Particular we like to build things in a way that embraces our open source community. Documentation is no different and as such is hosted on GitHub for everyone to contribute.', selected: false },
              { id: 'customers', title: 'In good company', description: "Companies across all industry verticals, from the smallest startups to the Global 2000, rely on our platform every day.", selected: false },
              { id: 'platform', title: 'The .NET Service Platform', description: "Build better .NET service solutions using an integrated and comprehensive platform. Focus on developing your solution's unique features, while enjoying the out-of-the-box benefits of the Particular Service Platform.", selected: false }
            ];

            $scope.orders = [];

            $scope.ordersReceived = [];

            var ordersHub = $.connection.ordersHub;

            ordersHub.client.orderReceived = function (data) {
                var selectedProductTitles = [];

                for (var i = 0; i < data.ProductIds.length; i++) {
                    var id = data.ProductIds[i];
                    for (var j = 0; j < $scope.products.length; j++) {
                        if ($scope.products[j].id === id) {
                            selectedProductTitles.push($scope.products[j].title);
                            break;
                        }
                    }
                }

                $scope.$apply(function (scope) {
                    scope.orders.push({ number: data.OrderNumber, titles: selectedProductTitles, status: 'Pending' });
                });

                $('#userWarning')
                    .css({ opacity: 0 })
                    .animate({ opacity: 1 }, 700);
            };

            ordersHub.client.orderCancelled = function (data) {
                $scope.$apply(function (scope) {
                    var idx = retrieveOrderIndex(scope, data.OrderNumber);
                    if (idx >= 0) {
                        scope.orders[idx].status = 'Cancelled';
                    }
                });
            };

            ordersHub.client.orderReady = function (data) {
                var items = [];

                for (var i = 0; i < data.ProductUrls.length; i++) {
                    var item = data.ProductUrls[i];

                    for (var j = 0; j < $scope.products.length; j++) {

                        if ($scope.products[j].id === item.Id) {
                            items.push({ url: item.Url, title: $scope.products[j].title });
                            break;
                        }
                    }
                }

                $scope.$apply(function (scope) {
                    var idx = retrieveOrderIndex(scope, data.OrderNumber);
                    if (idx >= 0) {
                        scope.orders[idx].status = 'Complete';
                    }
                    scope.ordersReceived.push({ number: data.OrderNumber, items: items });
                });
            };

            $.connection.hub.start();

            $scope.cancelOrder = function (number) {
                $scope.errorMessage = null;

                var idx = retrieveOrderIndex($scope, number);
                if (idx >= 0) {
                    $scope.orders[idx].status = 'Cancelling';
                }

                ordersHub.state.debug = $scope.debug;
                ordersHub.server.cancelOrder(number)
                    .fail(function () {
                        $scope.errorMessage = "We couldn't cancel you order, ensure all endpoints are running and try again!";
                    });
            };

            $scope.placeOrder = function () {

                $scope.errorMessage = null;

                var selectedProducts = [];
                angular.forEach($scope.products, function (product) {
                    if (product.selected) {
                        selectedProducts.push(product.id);
                    }
                });

                if (selectedProducts.length === 0) {
                    return;
                }

                ordersHub.state.debug = $scope.debug;
                ordersHub.server.placeOrder(selectedProducts)
                    .done(function () {
                        angular.forEach($scope.products, function (product) {
                            product.selected = false;
                        });
                    })
                    .fail(function () {
                        $scope.errorMessage = "We couldn't place you order, ensure all endpoints are running and try again!";
                    });
            };

            function retrieveOrderIndex(scope, orderNumber) {
                var idx = 0;

                for (; idx < scope.orders.length; idx++) {
                    if (scope.orders[idx].number === orderNumber) {
                        return idx;
                    }
                }

                return -1;
            }
        });

}())