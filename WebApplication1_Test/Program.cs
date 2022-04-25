using Microsoft.AspNetCore.Localization;
using System.Globalization;

CultureInfo[] supportedCultures = new CultureInfo[]
    {
        new CultureInfo("en-US"),
        new CultureInfo("ru-RU")
    };

#region builder
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews() // Локализация.
    .AddDataAnnotationsLocalization()
    .AddViewLocalization();

// Локализация.
builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");
builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    options.DefaultRequestCulture = new RequestCulture("ru-RU");
    options.SupportedCultures = supportedCultures;
    options.SupportedUICultures = supportedCultures;
});

// Telerik.
builder.Services.AddKendo();

// Отмена автоматического перевода JSON в camal-case в HTTP-response.
builder.Services.AddMvc()
    .AddJsonOptions(options => options.JsonSerializerOptions.PropertyNamingPolicy = null);
#endregion

#region app
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseRequestLocalization(new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture("ru-RU"),
    SupportedCultures = supportedCultures,
    SupportedUICultures = supportedCultures
});

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
#endregion

app.Run();
