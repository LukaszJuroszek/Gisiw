using Graph.Component.Models.CanvasJs;
using Microsoft.JSInterop;
using System.Threading.Tasks;

namespace Graph.Component.JsInterop
{
    public class CanvasJsChartJsInterop
    {
        public static ValueTask<string> CreateChart(IJSRuntime jsRuntime, string containerId, ICanvasJsConfig config)
        {
            return jsRuntime.InvokeAsync<string>(
                "graphComponents.createCanvasJsChart",
                containerId,
                config);
        }
    }
}
