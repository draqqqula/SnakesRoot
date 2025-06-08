using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.StaticFiles;
using ServerEngine;
using SessionApi.Filters;
using SessionApi.Services;
using SnakeGame;
using System.Net;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddServices();
builder.Services.AddGameApplication();
builder.Services.AddGameLauncher();


var app = builder.Build();

// <snippet_UseWebSockets>
var webSocketOptions = new WebSocketOptions
{
    KeepAliveInterval = TimeSpan.FromMinutes(2)
};

var staticContentTypeProvider = new FileExtensionContentTypeProvider();
staticContentTypeProvider.Mappings.Add(".data", "application/octet-stream");
var staticOptions = new StaticFileOptions()
{
    ContentTypeProvider = staticContentTypeProvider
};

app.UseCors();
app.UseDefaultFiles();
app.UseStaticFiles(staticOptions);
app.UseOptions();
app.UseWebSockets(webSocketOptions);
// </snippet_UseWebSockets>
//app.UseHttpsRedirection();

app.MapControllers();

app.Run();
