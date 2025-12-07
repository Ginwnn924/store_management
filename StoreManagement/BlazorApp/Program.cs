using BlazorApp.Components;
using BlazorApp.Services;
using Microsoft.JSInterop;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Configure HttpClient for API calls
builder.Services.AddHttpClient<IProductService, ProductService>(client =>
{
    client.BaseAddress = new Uri(builder.Configuration["ApiSettings:BaseUrl"] ?? "http://localhost:5163");
    client.Timeout = TimeSpan.FromSeconds(30);
});

builder.Services.AddHttpClient<ICategoryService, CategoryService>(client =>
{
    client.BaseAddress = new Uri(builder.Configuration["ApiSettings:BaseUrl"] ?? "http://localhost:5163");
    client.Timeout = TimeSpan.FromSeconds(30);
});

// AuthService - Scoped để share state trong cùng một circuit/session
builder.Services.AddScoped<IAuthService>(sp =>
{
    var httpClientFactory = sp.GetRequiredService<IHttpClientFactory>();
    var httpClient = httpClientFactory.CreateClient("AuthClient");
    var logger = sp.GetRequiredService<ILogger<AuthService>>();
    var jsRuntime = sp.GetRequiredService<IJSRuntime>();
    return new AuthService(httpClient, logger, jsRuntime);
});

builder.Services.AddHttpClient("AuthClient", client =>
{
    client.BaseAddress = new Uri(builder.Configuration["ApiSettings:BaseUrl"] ?? "http://localhost:5163");
    client.Timeout = TimeSpan.FromSeconds(30);
});

// Cart Service - Scoped để mỗi user có cart riêng
builder.Services.AddScoped<ICartService, CartService>();

// Toast Service - Singleton để share giữa các components
builder.Services.AddSingleton<IToastService, ToastService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();


app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
