using Graph.Component.Models.Graph.Data;
using Graph.Component.Models.Graph.Options;
using Microsoft.JSInterop;
using System.Threading.Tasks;

namespace Graph.Component.JsInterop
{
    public class GraphChartJsInterop
    {
        public static ValueTask<string> CreateGraph(IJSRuntime jsRuntime, string containerId, GraphData data, GraphOptions options)
        {
            return jsRuntime.InvokeAsync<string>(
                "graphComponents.createGraph",
                containerId,
                data,
                options);
        }
    }
}
