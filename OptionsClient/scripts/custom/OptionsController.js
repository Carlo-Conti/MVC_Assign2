(function () {


    var app = angular.module('optionsViewer', []);

    app.controller('OptionsController', ['$scope', '$http', function ($scope, $http) {

        $http.get('http://localhost:52543/api/yearterms')
        .success(function (data) {

            $scope.yearterms = data;

        });
    }]);



        

})();