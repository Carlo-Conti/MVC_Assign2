
(function () {
var myApp = angular.module('optionsViewer', ['ngRoute']);

myApp.config(function ($routeProvider) {

    $routeProvider

    .when('/', {
        templateUrl: 'views/login.html',
        controller: 'OptionsController'
    })

    .when('/register', {
        templateUrl: 'views/register.html',
        controller: 'OptionsController'
    })
    .when('/submitted', {
        templateUrl: 'views/submitted.html',
        controller: 'OptionsController'
    })
    .when('/optionselect', {
        templateUrl: 'views/optionselect.html',
        controller: 'OptionsController'
    })
    .otherwise({ redirectTo: "/login" });



});

myApp.controller('OptionsController', ['$scope', '$http', function ($scope, $http) {

    $scope.message = "in the view yo";


}]);

}());
