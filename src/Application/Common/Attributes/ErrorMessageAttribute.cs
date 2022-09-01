using System.Net;
using Application.Common.Constants;

namespace Application.Common.Attributes;

[AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
public sealed class ErrorMessageAttribute : Attribute
{
    public ErrorMessageAttribute(string description, HttpStatusCode code = HttpStatusCode.BadRequest)
    {
        Code = code;
        Description = description;
    }
    public HttpStatusCode Code { get; }
    public string Description { get; }
}

internal static class ResponseCodeExtension
{
    public static HttpStatusCode GetStatusCode(this Enum responseCode)
    {
        var attribute = GetAttribute(responseCode);

        if (attribute == null) return HttpStatusCode.BadRequest;

        return attribute.Code;
    }

    public static string GetDescription(this Enum responseCode)
    {
        var attribute = GetAttribute(responseCode);

        if (attribute == null) return ErrorMessageConstant.GENERAL_ERROR;

        return attribute.Description;
    }

    private static ErrorMessageAttribute? GetAttribute(Enum responseCode)
    {
        Type type = responseCode.GetType();
        var property = type.GetField(responseCode.ToString());
        if (property == null) return null;
        return ((ErrorMessageAttribute[])property.GetCustomAttributes(typeof(ErrorMessageAttribute), false)).FirstOrDefault();
    }
}
