using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarnetDigital.Models
{
    public class BusinessLogicResponse
    {
        public int StatusCode { get; set; } = 200;
        public string Message { get; set; } = null!;
        public object? Data { get; set; }

        public BusinessLogicResponse() { }

        public BusinessLogicResponse(int statusCode, string message)
        {
            StatusCode = statusCode;
            Message = message;
        }

        public BusinessLogicResponse(int statusCode, string message, object? data)
        {
            StatusCode = statusCode;
            Message = message;
            Data = data;
        }

    }
}
