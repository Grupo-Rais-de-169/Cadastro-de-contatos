using API.Contatos.Configuration;
using API.Contatos.Services;
using Microsoft.Extensions.Caching.Memory;
using NLog.Web;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddMemoryCache();
builder.Services.AddControllers();
builder.Services
    .AddSingleton<TokenServices>();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerConfiguration();
builder.AddJwtConfiguration();
builder.Host.UseNLog();
var app = builder.Build();
var cache = app.Services.GetRequiredService<IMemoryCache>();
cache.Set("key", "value", new MemoryCacheEntryOptions
{
    AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(8)
});

app.UseSwaggerConfiguration();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
