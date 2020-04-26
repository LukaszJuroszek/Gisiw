import vis from 'vis';

export function createGraph(continerId, data, options) {
    var container = document.getElementById(continerId);
    var network = new vis.Network(container, data, options);
    return network;
}