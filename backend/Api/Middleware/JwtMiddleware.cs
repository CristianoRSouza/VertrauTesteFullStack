using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Api.Middleware;

public class JwtMiddleware
{
    private readonly RequestDelegate _next;

    public JwtMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var token = context.Request.Headers["Authorization"].FirstOrDefault();
        
        if (!string.IsNullOrEmpty(token) && !token.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
        {
            context.Request.Headers["Authorization"] = $"Bearer {token}";
        }

        await _next(context);
    }
}
