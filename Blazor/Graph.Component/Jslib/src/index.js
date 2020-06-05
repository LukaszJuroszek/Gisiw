import vis from 'vis';
import CanvasJS  from './canvasjs.min';

export function createGraph(continerId, data, options) {
    var container = document.getElementById(continerId);
    var network = new vis.Network(container, data, options);
    return network;
}

export function createCanvasJsChart(continerId, config) {
    var iterationChart = new CanvasJS.Chart(continerId, config);
    iterationChart.render();
}



