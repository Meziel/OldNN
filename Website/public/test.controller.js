(function () {
    'use strict';

    angular
        .module('app')
        .controller('test', test);

    test.$inject = ['$http'];

    function test($http) {

        var vm = this;
        vm.ticksTrain = 10;
        vm.ticksTest = 10;
        vm.inputSize = 10;
        vm.generations = 10;
        vm.stockName = 'BTC';
        vm.training = false;
        vm.statistics = [];

        vm.train = train;

        activate();

        function activate() { }

        function train() {
            vm.training = true;
            $http.post('api/evolutionsimulator/train',
                {
                    ticksTrain: vm.ticksTrain,
                    ticksTest: vm.ticksTest,
                    inputSize: vm.inputSize,
                    generations: vm.generations,
                    stockName: vm.stockName
                }
            ).then(function (response) {
                vm.training = false;
                vm.statistics = response.data;
                createPlot();
            });
        }

        function createPlot() {

            var trace1 = {
                x: [],
                y: [],
                type: 'scatter',
                name: 'Training Accuracy'
            };

            var trace2 = {
                x: [],
                y: [],
                type: 'scatter',
                name: 'Testing Accuracy'
            };

            var trace3 = {
                x: [],
                y: [],
                type: 'scatter',
                name: 'Training Average'
            };

            var trace4 = {
                x: [],
                y: [],
                type: 'scatter',
                name: 'Testing Average'
            };

            vm.statistics.trainStatistics.forEach(function (statistic) {
                trace1.x.push(statistic.Generation);
                trace1.y.push(statistic.Performance);
            });

            vm.statistics.testStatistics.forEach(function (statistic) {
                trace2.x.push(statistic.Generation);
                trace2.y.push(statistic.Performance);
            });

            vm.statistics.trainAverage.forEach(function (statistic) {
                trace3.x.push(statistic.Generation);
                trace3.y.push(statistic.Performance);
            });

            vm.statistics.testAverage.forEach(function (statistic) {
                trace4.x.push(statistic.Generation);
                trace4.y.push(statistic.Performance);
            });
           
            var data = [trace1, trace2];
            var data2 = [trace3, trace4]

            var layout = {

                title: 'Performance',
                xaxis: {
                    title: 'Generation',
                    showgrid: false
                },
                yaxis: {
                    title: 'Performance'
                }
            };

            Plotly.newPlot('plot', data, layout);
            Plotly.newPlot('plot2', data2, layout);
        }
    }
})();
