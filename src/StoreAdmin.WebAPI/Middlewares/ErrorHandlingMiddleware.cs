using System.Security.Authentication;

namespace StoreAdmin.WebAPI.Middlewares
{
    public class ErrorHandlingMiddleware : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                // Call the next middleware in the pipeline
                await next(context);
            }
            catch (UnauthorizedAccessException)
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("Unauthorized");
            }
            catch (AuthenticationException)
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("Unauthorized");
            }
            catch (ArgumentException ae)
            {
                // Handle the error and write a custom error response
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync($"{{ \"error\": \"{ae.Message}\" }}");
            }
            catch (Exception ex)
            {
                // Handle the error and write a custom error response
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync($"{{ \"error\": \"{ex.Message}\" }}");
            }
        }
    }
}

