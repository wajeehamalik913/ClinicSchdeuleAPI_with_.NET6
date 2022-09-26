﻿//using Newtonsoft.Json;
//using System.ComponentModel.DataAnnotations;
//using System.Net;

//namespace ClinicApi.Middleware
//{
//    public class ExceptionHandlerMiddleware
//    {
//        private const string JsonContentType = "application/json";
//        private readonly RequestDelegate request;

//        /// <summary>
//        /// Initializes a new instance of the <see cref="ExceptionHandlerMiddleware"/> class.
//        /// </summary>
//        /// <param name="next">The next.</param>
//        public ExceptionHandlerMiddleware(RequestDelegate next)
//        {
//            this.request = next;
//        }

//        /// <summary>
//        /// Invokes the specified context.
//        /// </summary>
//        /// <param name="context">The context.</param>
//        /// <returns></returns>
//        public Task Invoke(HttpContext context) => this.InvokeAsync(context);

//        async Task InvokeAsync(HttpContext context)
//        {
//            try
//            {
//                await this.request(context);
//            }
//            catch (Exception exception)
//            {
//                var httpStatusCode = ConfigurateExceptionTypes(exception);

//                // set http status code and content type
//                context.Response.StatusCode = httpStatusCode;
//                context.Response.ContentType = JsonContentType;

//                // writes / returns error model to the response
//                await context.Response.WriteAsync(
//                    JsonConvert.SerializeObject(new ErrorModelViewModel
//                    {
//                        Message = exception.Message
//                    }));

//                context.Response.Headers.Clear();
//            }
//        }

//        /// <summary>
//        /// Configurates/maps exception to the proper HTTP error Type
//        /// </summary>
//        /// <param name="exception">The exception.</param>
//        /// <returns></returns>
//        private static int ConfigurateExceptionTypes(Exception exception)
//        {
//            int httpStatusCode;

//            // Exception type To Http Status configuration 
//            switch (exception)
//            {
//                case var _ when exception is ValidationException:
//                    httpStatusCode = (int)HttpStatusCode.BadRequest;
//                    break;
//                default:
//                    httpStatusCode = (int)HttpStatusCode.InternalServerError;
//                    break;
//            }

//            return httpStatusCode;
//        }
//    }
//}
