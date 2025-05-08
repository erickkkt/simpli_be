

namespace Simpli.SearchPortal.Api.Helper
{
    /// <summary>
    /// This class is used to help with application configuration.
    /// </summary>
    public static class ApplicationHelper
    {
        public static void AddSecurityHeaders(this IApplicationBuilder app)
        {
            // Custom implementation for X-Content-Type-Options header
            app.Use(async (context, next) =>
            {
                context.Response.Headers["X-Content-Type-Options"] = "nosniff";
                await next();
            });

            // Custom implementation for HSTS
            app.Use(async (context, next) =>
            {
                context.Response.Headers["Strict-Transport-Security"] = "max-age=31536000; includeSubDomains";
                await next();
            });

            // Custom implementation for X-XSS-Protection
            app.Use(async (context, next) =>
            {
                context.Response.Headers["X-XSS-Protection"] = "1; mode=block";
                await next();
            });

            // Custom implementation for X-Frame-Options
            app.Use(async (context, next) =>
            {
                context.Response.Headers["X-Frame-Options"] = "SAMEORIGIN";
                await next();
            });

            // Custom implementation for Content-Security-Policy
            app.Use(async (context, next) =>
            {
                context.Response.Headers["Content-Security-Policy"] = "default-src 'self' data: https:; style-src * 'unsafe-inline'; script-src 'self' * 'unsafe-inline'; font-src *; img-src *";
                await next();
            });

            // Custom implementation for Referrer-Policy
            app.Use(async (context, next) =>
            {
                context.Response.Headers["Referrer-Policy"] = "strict-origin";
                await next();
            });
        }
    }
}
