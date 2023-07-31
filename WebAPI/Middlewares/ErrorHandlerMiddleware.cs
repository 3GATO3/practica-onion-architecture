
using Aplication.Wrappers;
using Newtonsoft.Json;
using System.Net;
using System.Text.Json;
using System.Web.Helpers;

namespace WebAPI.Middlewares
{
    public class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public ErrorHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                if (!context.Response.HasStarted)
                {
                    await _next(context);
                }
                else
                {
                    await context.Response.WriteAsync(Json.Encode( string.Empty));
                }
                
            }
            catch (Exception error)
            {
                var response = context.Response;
                response.ContentType = "application/json";
                var responseModel = new Response<string>()
                {
                    Succeded = false,
                    Message = error?.Message
                };
                switch (error)
                {
                    case Aplication.Exceptions.ApiException e:
                        response.StatusCode = (int)HttpStatusCode.BadRequest;
                        
                        break;
                    case Aplication.Exceptions.ValidationException e:
                        response.StatusCode = (int)HttpStatusCode.BadRequest;
                        responseModel.Errors = e.Errors;

                        break;
                    case KeyNotFoundException e:
                        response.StatusCode = (int)HttpStatusCode.NotFound;

                        break;

                    default:
                        response.StatusCode = (int)HttpStatusCode.InternalServerError;

                        break;
                }

                var result = System.Text.Json.JsonSerializer.Serialize(responseModel);
                await response.WriteAsync(result);
                
            }

        }
    }
}
