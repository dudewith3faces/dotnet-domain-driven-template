using System.Net;
using Application.Common.Attributes;

namespace Application.Common.Enums;

public enum ErrorMessageEnum
{
    [ErrorMessageAttribute("Test error")]
    TEST
}
