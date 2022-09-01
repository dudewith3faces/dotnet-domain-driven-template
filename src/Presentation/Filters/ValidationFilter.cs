using Application.Common.Constants;
using Application.Common.Exceptions;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Presentation.Filters;

public class ValidationFilter : IAsyncActionFilter
{
    public Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        if (context.ModelState.IsValid) return next();

        var keys = new List<string>();
        int i = 0;
        IDictionary<string, IEnumerable<string>> errors = new Dictionary<string, IEnumerable<string>>();

        foreach (var key in context.ModelState.Keys)
            keys.Add(Char.ToLowerInvariant(key[0]) + key.Substring(1));

        foreach (var item in context.ModelState.Values)
        {
            errors.Add(keys[i], item.Errors.Select(s => s.ErrorMessage));
            i++;
        }

        throw new BaseException(ErrorMessageConstant.VALIDATION_ERROR, errors);
    }
}