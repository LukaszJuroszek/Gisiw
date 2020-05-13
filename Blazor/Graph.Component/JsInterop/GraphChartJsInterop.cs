using Graph.Component.Models.Graph.Data;
using Graph.Component.Models.Graph.Options;
using Microsoft.JSInterop;
using System.Threading.Tasks;

namespace Graph.Component.JsInterop
{
    public class GraphChartJsInterop
    {
        public async static ValueTask<string> CreateGraphAsync(IJSRuntime jsRuntime, string containerId, IGraphData data, IGraphOptions options)
        {
            return await jsRuntime.InvokeAsync<string>("graphComponents.createGraph", containerId, data, options);
        }
    }
}
