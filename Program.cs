using BlazorPattern;
using BlazorPattern.Services;

var builder = WebApplication.CreateBuilder(args);

// Blazor Web App (.NET 8 unified model): components render as static SSR by default and
// opt in to interactivity per page/component with @rendermode (see docs/ARQUITETURA.md).
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents(options =>
    {
        // Surface exception details to the browser console only while developing.
        options.DetailedErrors = builder.Environment.IsDevelopment();
    });

// Compress HTML/CSS/JS responses. HTTPS compression stays off (framework default) to avoid BREACH.
builder.Services.AddResponseCompression();

builder.Services.AddHealthChecks();
builder.Services.AddPatternServices();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    // createScopeForErrors lets the error page resolve scoped services (e.g. for logging).
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseResponseCompression();
app.UseHttpsRedirection();
app.UseStaticFiles();

// Required by the antiforgery protection that Blazor applies to every <EditForm>/form post.
app.UseAntiforgery();

app.MapHealthChecks("/health");
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
