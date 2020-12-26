using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using System.Web.Http.ModelBinding;

namespace FriMav.Delivery.Api
{
    class ModelStateValidationFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            if (!actionContext.ModelState.IsValid)
            {
                actionContext.Response = actionContext.Request.CreateResponse(
                    HttpStatusCode.BadRequest, new ApiResourceValidationError(
                                                        ApiResourceValidationError.ErrorMessage,
                                                        SerializeModelState(actionContext.ModelState)));
            }
        }

        private IEnumerable<string> SerializeModelState(ModelStateDictionary modelState)
        {
            var errorList = new List<string>();

            foreach (var keyModelStatePair in modelState)
            {
                var key = keyModelStatePair.Key;

                var errors = keyModelStatePair.Value.Errors;

                if (errors != null && errors.Count > 0)
                {
                    IEnumerable<string> errorMessages = errors.Select(
                        error => string.IsNullOrEmpty(error.ErrorMessage)
                                     ? ApiResourceValidationError.MissingPropertyError
                                     : error.ErrorMessage).ToArray();

                    errorList.AddRange(errorMessages);
                }
            }
            return errorList;
        }
    }
}
