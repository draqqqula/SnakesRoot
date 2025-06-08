using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;

namespace SessionApi.Filters;

public class OptionsMiddleware
{
    private readonly RequestDelegate _next;

    public OptionsMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public Task Invoke(HttpContext context)
    {
        return BeginInvoke(context);
    }

    private Task BeginInvoke(HttpContext context)
    {
        if (context.Request.Method == "OPTIONS")
        {
            var requestHeaders = context.Request.Headers["Access-Control-Request-Headers"];
            Console.WriteLine(context.Request.Path);
            Console.WriteLine(requestHeaders);
            context.Response.Headers.Add("Access-Control-Allow-Origin", new[] { "*" });
            context.Response.Headers.Add("Access-Control-Allow-Headers", requestHeaders);
            context.Response.Headers.Add("Access-Control-Allow-Methods", new[] { "GET, POST, PUT, DELETE, OPTIONS" });
            context.Response.Headers.Add("Access-Control-Allow-Credentials", new[] { "true" });
            context.Response.StatusCode = 200;
            return context.Response.WriteAsync("OK");
        }
        else
        {
            var requestHeaders = context.Request.Headers["Access-Control-Request-Headers"];
            Console.WriteLine(context.Request.Path);
            Console.WriteLine(requestHeaders);
            context.Response.Headers.Add("Access-Control-Allow-Origin", new[] { "*" });
            context.Response.Headers.Add("Access-Control-Allow-Headers", requestHeaders);
            context.Response.Headers.Add("Access-Control-Allow-Methods", new[] { "GET, POST, PUT, DELETE, OPTIONS" });
            context.Response.Headers.Add("Access-Control-Allow-Credentials", new[] { "true" });
            context.Response.StatusCode = 200;
            return _next.Invoke(context);
        }

        return _next.Invoke(context);
    }
}

public static class OptionsMiddlewareExtensions
{
    public static IApplicationBuilder UseOptions(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<OptionsMiddleware>();
    }
}