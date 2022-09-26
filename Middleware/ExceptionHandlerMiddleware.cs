using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Net;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.Net.Http;
using Microsoft.JSInterop;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Logging;

namespace ClinicApi.Middleware
{
    public class ExceptionHandlerMiddleware
    {
       // private readonly ILogger _logger;
        public ExceptionHandlerMiddleware(RequestDelegate next/*,ILogger logger*/)
        {
            this.Next = next;
            //_logger = logger;
        }
        public RequestDelegate Next { get; private set; }

        public async Task Invoke(HttpContext httpContext) 
        { 
            try
            {
                await Next(httpContext);
            }
            catch (Exception error)
            {
                var response = httpContext.Response;
                response.ContentType = "application/json";

                switch (error)
                {
                    case TimeoutException e:
                        //_logger.LogError("timed out");
                        response.StatusCode = (int)HttpStatusCode.GatewayTimeout;
                        break;
                    
                    case OutOfMemoryException e:
                        //_logger.LogError("Out of memory!!! You have insufficient storage");
                        response.StatusCode = (int)HttpStatusCode.InsufficientStorage;
                        break;

                    case KeyNotFoundException e:
                        //_logger.LogError("Not Found");
                        response.StatusCode = (int)HttpStatusCode.NotFound;
                        break;

                    case SecurityTokenExpiredException e:
                        //_logger.LogError("The token is expired");
                        httpContext.Response.Headers.Add("Token-Expired", "true");
                       
                        break;

                    case UnauthorizedAccessException e:
                        //_logger.LogError("Unuthorized to access this resource");
                        response.StatusCode = (int)HttpStatusCode.Unauthorized;
                        break;

                    default:
                        
                        response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        ///_logger.LogError(response.ToString());
                        break;
                }
                
            }
            
            //await Next(httpContext);
        }
    }
}
