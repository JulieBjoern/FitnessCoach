using FitnessCoach.Components;
using MudBlazor.Services;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Serilog takes over logging. The default providers are cleared first, so the console
// doesn't get every log line twice (once from Serilog, once from Microsoft's console logger).
builder.Logging.ClearProviders();

// Aspire: OpenTelemetry (logs, traces, metrics to the dashboard), health checks,
// service discovery and HTTP resilience.
builder.AddServiceDefaults();

// writeToProviders: true forwards Serilog's events to the remaining ILoggerProviders —
// here the OpenTelemetry provider from AddServiceDefaults(). Without it, logs would no
// longer show up under "Structured logs" in the Aspire dashboard.
builder.Services.AddSerilog((services, logger) => logger
        .ReadFrom.Configuration(builder.Configuration)
        .WriteTo.Console()
        .WriteTo.File("logs/fitnesscoach-.log", rollingInterval: RollingInterval.Day),
    writeToProviders: true);

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddMudServices();

var app = builder.Build();

app.MapDefaultEndpoints();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Logger.LogInformation("FitnessCoach started in {Environment} environment", app.Environment.EnvironmentName);

app.Run();
