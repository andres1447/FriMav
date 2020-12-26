using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FriMav.Delivery.Api
{
    public class ApiResourceValidationError
    {
        public const string ErrorMessage = "Solicitud invalida.";
        public const string MissingPropertyError = "Error indefinido.";

        public string Message { get; private set; }
        public IEnumerable<string> Errors { get; private set; }

        public ApiResourceValidationError(string message, IEnumerable<string> errors)
        {
            Message = message;
            Errors = errors;
        }
    }
}
