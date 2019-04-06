using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using SuperDigital.DigitalAccount.CrossCutting;
using System;
using System.Net;

namespace SuperDigital.DigitalAccount.Api.Filters
{
    public class CustomExceptionFilter : IExceptionFilter
    {
        private readonly ILogger<CustomExceptionFilter> logger;
        private readonly IHostingEnvironment hostingEnvironment;

        public CustomExceptionFilter(
            ILogger<CustomExceptionFilter> logger,
            IHostingEnvironment hostingEnvironment)
        {
            this.logger = logger;
            this.hostingEnvironment = hostingEnvironment;
        }

        public void OnException(ExceptionContext context)
        {
            int statusCode = (int)HttpStatusCode.InternalServerError;

            context.HttpContext.Response.StatusCode = statusCode;

            BaseResponse<Exception> result = new BaseResponse<Exception>();

            if (hostingEnvironment.IsProduction())
            {
                result.Result = null;
                result.Errors = new System.Collections.Generic.List<ErrorResponse>
                {
                    new ErrorResponse
                    {
                        Code = statusCode,
                        Message = "Houve um erro inesperado"
                    }
                };
            }
            else
            {
                result.Result = context.Exception;
                result.Errors = new System.Collections.Generic.List<ErrorResponse>
                {
                    new ErrorResponse
                    {
                        Code = statusCode,
                        Message = "Houve um erro inesperado"
                    }
                };
            }

            context.Result = new JsonResult(result);
        }
    }
}