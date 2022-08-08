using AutoScales.Client;
using AutoScales.Client.Providers;
using AutoScales.Client.Services.Implementation;
using AutoScales.Client.Services.Interfaces;
using AutoScales.Shared.State;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");



builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IWeighingService, WeighingService>();
builder.Services.AddScoped<IJournalService, JournalService>();
builder.Services.AddScoped<ChartState>();
builder.Services.AddScoped<TransportState>();
builder.Services.AddScoped<JournalState>();
builder.Services.AddOptions();
builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<CustomStateProvider>();

builder.Services.AddScoped<AuthenticationStateProvider>(s => s.GetRequiredService<CustomStateProvider>());

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

await builder.Build().RunAsync();
