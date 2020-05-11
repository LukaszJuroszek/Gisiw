using System;
using System.Net.Http;
using System.Threading.Tasks;
using Graph.Core.Services;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Graph
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);

            builder.RootComponents.Add<App>("app");
            
            builder.Services.AddSingleton(new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

            builder.Services.AddSingleton<IChromosomeService,ChromosomeService>();
            builder.Services.AddSingleton<IEvolutionService, EvolutionService>();
            builder.Services.AddSingleton<IGraphChartService, GraphChartService>();
            builder.Services.AddSingleton<IGraphConsistentService, GraphConsistentService>();
            builder.Services.AddSingleton<IMatrixService, MatrixService>();
            builder.Services.AddSingleton<IPopulationService, PopulationService>();
            builder.Services.AddSingleton<ICanvasJsChartService, CanvasJsChartService>();
            
            await builder.Build().RunAsync();
        }
    }
}
