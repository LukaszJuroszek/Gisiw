import * as CanvasJS from 'canvasjs';

window.onload = function () {
    var sumChart = new CanvasJS.Chart("sumChart", {
        animationEnabled: true,
        theme: "light2",
        title: {
            text: "Simple Line Chart"
        },
        axisY: {
            includeZero: false
        },
        data: [{
            type: "line",
            dataPoints: [
                { y: 450 },
                { y: 414 },
                { y: 520 },
                { y: 480 },
                { y: 510 }
            ]
        }]
    });
    sumChart.render();
    var sumChart2 = new CanvasJS.Chart("sumChart2", {
        animationEnabled: true,
        theme: "light2",
        title: {
            text: "Simple Line Chart2"
        },
        axisY: {
            includeZero: false
        },
        data: [{
            type: "line",
            dataPoints: [
                { y: 450 },
                { y: 414 },
                { y: 520 },
                { y: 480 },
                { y: 510 }
            ]
        }]
    });
    sumChart2.render();
    var paretoChart = new CanvasJS.Chart("paretoChart",
        {
            title: {
                text: "Pareto front"
            },
            axisX:{
                gridThickness: 1 ,  
                interval: 1,
              },
              axisY:{        
                interval: 1,
              },
            data: [
                {
                    type: "scatter",
                    dataPoints: [
                        { x: 2, y: 3 },
                        { x: 3, y: 2 },
                        { x: 2, y: 4 },
                        { x: 4, y: 2 },
                        { x: 2, y: 4 },
                        { x: 4, y: 5 },
                        { x: 4, y: 6 },
                        { x: 4, y: 2 },
                        { x: 4, y: 2 },
                        { x: 1, y:4 },
                        { x: 1, y: 2 },
                        { x: 1, y: 2 },
                        { x: 4, y: 2 },
                        { x: 5, y: 6 },
                    ]
                }
            ]
        });
    paretoChart.render();
}