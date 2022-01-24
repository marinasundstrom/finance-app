using System.Globalization;

using Accounting.Client;

using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

using MudBlazor;
using MudBlazor.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.Services.AddMudServices(config =>
{
    config.SnackbarConfiguration.PositionClass = Defaults.Classes.Position.BottomRight;

    config.SnackbarConfiguration.PreventDuplicates = false;
    config.SnackbarConfiguration.NewestOnTop = false;
    config.SnackbarConfiguration.ShowCloseIcon = true;
    config.SnackbarConfiguration.VisibleStateDuration = 10000;
    config.SnackbarConfiguration.HideTransitionDuration = 500;
    config.SnackbarConfiguration.ShowTransitionDuration = 500;
    config.SnackbarConfiguration.SnackbarVariant = Variant.Filled;
});

CultureInfo? culture = new("sv-SE");
CultureInfo.DefaultThreadCurrentCulture = culture;
CultureInfo.DefaultThreadCurrentUICulture = culture;

builder.Services.AddHttpClient(nameof(IAccountsClient), (sp, http) =>
{
    http.BaseAddress = new Uri($"{builder.HostEnvironment.BaseAddress}api/");
})
.AddTypedClient<IAccountsClient>((http, sp) => new AccountsClient(http));

builder.Services.AddHttpClient(nameof(IVerificationsClient), (sp, http) =>
{
    http.BaseAddress = new Uri($"{builder.HostEnvironment.BaseAddress}api/");
})
.AddTypedClient<IVerificationsClient>((http, sp) => new VerificationsClient(http));

builder.Services.AddHttpClient(nameof(IEntriesClient), (sp, http) =>
{
    http.BaseAddress = new Uri($"{builder.HostEnvironment.BaseAddress}api/");
})
.AddTypedClient<IEntriesClient>((http, sp) => new EntriesClient(http));

await builder.Build().RunAsync();