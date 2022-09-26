using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace ClinicApi.Middleware
{
    public class ValidationHandlerMiddleware
    {
        public ValidationHandlerMiddleware(RequestDelegate next)
        {
            this.Next = next;
        }
        public RequestDelegate Next { get; private set; }
        public async Task Invoke(HttpContext context,ActionContext a_Context)
        {
            if (!a_Context.ModelState.IsValid)
            {
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            }
            await Next(context);
        }
    }
}
