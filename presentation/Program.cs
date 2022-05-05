using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

using presentation;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped<AppState>();
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://localhost:7022/") });

await builder.Build().RunAsync();
