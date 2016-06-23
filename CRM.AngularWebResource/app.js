var app = angular.module('app', ['ui.grid']);

app.controller('mainController', function($http, $scope, uiGridConstants) {
	    $scope.gridOptions = {
        enableSorting: true,
        enableFiltering: true,
        columnDefs: [
          { name: 'Name', width: '25%', field: 'name', sort: {
              direction: uiGridConstants.ASC,
              ignoreSort: true,
              priority: 0
          }, cellTemplate: '<div class="ui-grid-cell-contents"><a href="javascript://">{{row.entity["name"]}}</a></div>' },
          { name: 'Street', width: '30%', field: 'address1_line1' },
          { name: 'Postcode', width: '15%', field: 'address1_postalcode' },
          { name: 'City', width: '30%', field: 'address1_city' },
        ]
    };

	$http.get('/api/data/v8.0/accounts?$select=accountid,name,address1_line1,address1_city,address1_postalcode').then(function (result){
		$scope.gridOptions.data = result.data.value;
	});
});