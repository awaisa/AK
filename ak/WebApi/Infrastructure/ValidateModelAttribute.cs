using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace WebApiCore._Code
{
    public class ValidateModelAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.ModelState.IsValid == false)
            {
                context.Result = new BadRequestObjectResult(context.ModelState);
            }
        }
    }

    public class ModelState
    {
        public object Keys { get; set; }
    }

    public static class ModelStateDictionaryExtension
    {
        public static ModelState ToMyState(this ModelStateDictionary modelState)
        {
            //remove line and position numbers from exception message
            Func<string, string> FormatExceptionMessage = new Func<string, string>((string s) =>
            {
                Regex ex = new Regex(", line [0-9]+, position [0-9]+");
                s = ex.Replace(s, "");
                return s;
            });
            return new ModelState()
            {
                Keys = (from kvp in modelState
                        from e in kvp.Value.Errors
                        select new { kvp.Key, ErrorMessage = 
                                            e.Exception == null ? e.ErrorMessage : 
                                            string.Format("{0} {1}", FormatExceptionMessage(e.Exception.Message), e.ErrorMessage) })
                        .ToList()
            };
        }
    }
}
