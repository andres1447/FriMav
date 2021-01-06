using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;

namespace FriMav.Client
{
    public class CefFxResponse
    {
        static JsonSerializerSettings _settings = new JsonSerializerSettings()
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver()
        };

        public bool Success { get; set; }
        public string ErrorMessage { get; set; }

        public static string WrapResult(Action action)
        {
            try
            {
                action.Invoke();
                return JsonConvert.SerializeObject(new CefFxResponse { Success = true }, _settings);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        public static string WrapResult<T>(Func<T> action)
        {
            try
            {
                var result = action.Invoke();
                return JsonConvert.SerializeObject(new CefFxResult<T>(result), _settings);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        public static string HandleException(Exception ex)
        {
            return JsonConvert.SerializeObject(new CefFxResponse
            {
                Success = false,
                ErrorMessage = string.Format("{0}\n{1}", ex.Message, ex.StackTrace)
            }, _settings);
        }
    }

    public class CefFxResult<T> : CefFxResponse
    {
        public T Result { get; set; }

        public CefFxResult(T result)
        {
            Result = result;
            Success = true;
        }
    }
}
