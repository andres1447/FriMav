using FriMav.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Filters;

namespace FriMav.Delivery.Api
{
    public class ValidationExceptionFilterAttribute : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext context)
        {
            if (context.Exception is ValidationException)
            {
                var ex = context.Exception as ValidationException;
                context.Response = context.Request.CreateResponse(
                    HttpStatusCode.BadRequest, new ApiResourceValidationError(ex.Message, new List<string>
                    {
                        ex.Message
                    }));
            }
        }
    }
}
