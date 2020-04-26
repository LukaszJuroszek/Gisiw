using Graph.Component.Models.Data;
using Graph.Component.Models.Options;
using Microsoft.JSInterop;
using System.Threading.Tasks;

namespace Graph.Component.JsInterop
{
    public class GraphChartJsInterop
    {
        public static ValueTask<string> CreateGraph(IJSRuntime jsRuntime, string containerId, GraphData data, GraphOptions options)
        {
            return jsRuntime.InvokeAsync<string>(
                "graphChart.createGraph",
                containerId,
                data,
                options);
        }
    }
}
