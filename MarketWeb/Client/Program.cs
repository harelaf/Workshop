using MarketWeb.Client.Connect;
using MarketWeb.Client.Helpers;
using MarketWeb.Client.Services;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MarketWeb.Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("app");

            builder.Services.AddScoped<IMarketAPIClient, MarketAPIClient>()
                            .AddScoped<IHttpService, HttpService>()
                            .AddScoped<IAlertService, AlertService>();

            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

            var host = builder.Build();

            var marketAPIClient = host.Services.GetRequiredService<IMarketAPIClient>();
            await marketAPIClient.EnterSystem();

            await host.RunAsync();
        }
    }
}
