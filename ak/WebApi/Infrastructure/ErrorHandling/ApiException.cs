using BusinessCore.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace WebApiCore.Infrastructure.ErrorHandling
{

public class ApiException : Exception
{
    public int StatusCode { get; set; }

    public ValidationErrorCollection Errors { get; set; }

    public ApiException(string message, 
                        int statusCode = 500, 
                        ValidationErrorCollection errors = null) :
        base(message)
    {
        StatusCode = statusCode;
        Errors = errors;
    }
    public ApiException(Exception ex, int statusCode = 500) : base(ex.Message)
    {
        StatusCode = statusCode;
    }
}

}