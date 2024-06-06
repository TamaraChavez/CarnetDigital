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

        public BusinessLogicResponse() 
        { 
        }
        public BusinessLogicResponse( int statusCode, string message)
        {
            StatusCode = statusCode;
            Message = message;
        }

        public BusinessLogicResponse NoContent()
        {
            return new BusinessLogicResponse(204, "No Content");
        }

        public BusinessLogicResponse Success(string message)
        {
            return new BusinessLogicResponse(200, message);
        }

        public BusinessLogicResponse Error(int statusCode, string message)
        {
            return new BusinessLogicResponse(statusCode, message);
        }
    }
}
