using System.Net.Http.Headers;
using System.Text;
using Data;
using Microsoft.EntityFrameworkCore;

namespace WebAPI.Middlewares;

public class AuthMiddleware
{
    private readonly RequestDelegate _next;

    public AuthMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context) {

        if (context.Request.Headers.TryGetValue("Authorization", out var header))
        {
            var authHeader = AuthenticationHeaderValue.Parse(header);
            if (authHeader.Parameter != null)
            {
                var credBytes = Convert.FromBase64String(authHeader.Parameter);
                var credentials = Encoding.UTF8.GetString(credBytes).Split(':',2);
                var email = credentials[0];
                var password = credentials[1];

                using(ApplicationContext db = new ApplicationContext()) {
                    var user = await db.Users.Where(c => c.Email == email && c.Password == password).
                        FirstOrDefaultAsync(cancellationToken: default);
                    if (user != null)
                    {
                        context.Items["UserId"] = user.UserId;
                    }
                }
                
            }
        }

        await _next(context);
    }
}