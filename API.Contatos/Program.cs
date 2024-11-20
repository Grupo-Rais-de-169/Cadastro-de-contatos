using API.Contatos.Configuration;
using API.Contatos.Services;
using Microsoft.Extensions.Caching.Memory;
using NLog.Web;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using System.Security.Cryptography;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddMemoryCache();
builder.Services.AddControllers();
builder.Services
    .AddSingleton<TokenServices>();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerConfiguration();
builder.AddJwtConfiguration();

builder.Logging.ClearProviders();
builder.Host.UseNLog();

var app = builder.Build();
var cache = app.Services.GetRequiredService<IMemoryCache>();
cache.Set("key", "value", new MemoryCacheEntryOptions
{
    AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(8)
});


app.UseSwaggerConfiguration();


app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
