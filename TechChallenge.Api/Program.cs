using Microsoft.Extensions.Caching.Memory;
using TechChallenge.Api;


var builder = WebApplication.CreateBuilder(args);

builder.ConfigureServices();

var app = builder.Build();

var cache = app.Services.GetRequiredService<IMemoryCache>();
cache.Set("key", "value", new MemoryCacheEntryOptions
{
    AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1)
});

app.ConfigureMiddleware();

await app.RunAsync();