using Graph.Component.Models.CanvasJs;
using Microsoft.JSInterop;
using System.Threading.Tasks;

namespace Graph.Component.JsInterop
{
    public class CanvasJsChartJsInterop
    {
        public async static ValueTask<string> CreateChartAsync(IJSRuntime jsRuntime, string containerId, ICanvasJsConfig config)
        {
            return await jsRuntime.InvokeAsync<string>("graphComponents.createCanvasJsChart", containerId, config);
        }
    }
}
