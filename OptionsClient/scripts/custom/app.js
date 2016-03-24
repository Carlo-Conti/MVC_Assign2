var myApp = angular.module('myApp', ['ngRoute']);

myApp.config(function ($routeProvider) {

    $routeProvider

    .when('/', {
        templateUrl: 'views/login.html',
        controller: 'mainController'
    })

    .when('/register', {
        templateUrl: 'views/register.html',
        controller: 'secondController'
    })
    .when('/submitted', {
        templateUrl: 'views/submitted.html',
        controller: 'secondController'
    })
    .when('/optionselect', {
        templateUrl: 'views/optionselect.html',
        controller: 'secondController'
    })

});

myApp.controller('mainController', ['$scope', '$log', function ($scope, $log) {

    $scope.name = 'Main';

}]);

myApp.controller('secondController', ['$scope', '$log', function ($scope, $log) {

    $scope.name = 'Second';

}]);
