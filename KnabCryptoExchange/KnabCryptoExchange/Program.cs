using KnabCryptoExchange.Domain;
using KnabCryptoExchange.Models;
using KnabCryptoExchange.Service;
using Microsoft.Extensions.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

var marketCapConfig = new CoinMarketCapConfig();
builder.Configuration.GetSection(nameof(CoinMarketCapConfig)).Bind(marketCapConfig);
builder.Services.AddSingleton(marketCapConfig);

builder.Services.AddScoped<ICryptocurrencyReader, CryptocurrencyReader>();
builder.Services.AddScoped<ICryptoExchangeService, CryptoExchangeService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/CryptoExchange/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=CryptoExchange}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
