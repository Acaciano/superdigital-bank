using System.Collections.Generic;
using System.Linq;

namespace SuperDigital.DigitalAccount.CrossCutting
{
    public class BaseResponse<T> : BaseResponse<T, ErrorResponse>
    {
        public BaseResponse()
        {
            Errors = new List<ErrorResponse>();
        }
        public BaseResponse(T data)
        {
            Result = data;
            Errors = new List<ErrorResponse>();
        }
    }

    public class BaseResponse<T, TError>
    {
        public BaseResponse()
        {
            Errors = new List<TError>();
        }

        public BaseResponse(T data)
        {
            Result = data;
            Errors = new List<TError>();
        }

        public int StatusCode { get; set; } = 200;
        public bool Success => !Errors.Any();
        public T Result { get; set; }
        public List<TError> Errors { get; set; }
    }

    public class ErrorResponse
    {
        public int Code { get; set; }
        public string Message { get; set; }
    }
}
