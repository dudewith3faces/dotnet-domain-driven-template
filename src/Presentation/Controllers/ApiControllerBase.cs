using System.Net.Mime;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Presentation.DTOs.Response.Base;

namespace Presentation.Controllers;

[ApiVersion("1.0")]
[ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
[ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
[Produces(MediaTypeNames.Application.Json)]
public class ApiControllerBase : ControllerBase
{
    private ISender? _mediator;
    private ISender Mediator => _mediator ??= HttpContext.RequestServices.GetRequiredService<ISender>();
    protected async Task<TResponse> SendAsync<TRequest, TResponse>(TRequest request, CancellationToken cancellationToken) where TRequest : IRequest<TResponse> where TResponse : struct => (TResponse)(await Mediator.Send(request, cancellationToken));
    protected async Task SendAsync<TRequest>(TRequest request, CancellationToken cancellationToken) where TRequest : IRequest<Unit> => await SendAsync<TRequest, Unit>(request, cancellationToken);
    protected IActionResult CreateResponse() => Ok(new BaseResponse());
    protected IActionResult CreateResponse(object data) => Ok(new PayloadResponse<object>(data));
    protected IActionResult CreateResponse<T>(T data) where T : struct => Ok(new PayloadResponse<T>(data));
    protected IActionResult CreateResponse(IEnumerable<object> data, int pageSize, int pageNumber) => Ok(new PaginatedResponse<object>(data, pageSize, pageNumber));
    protected IActionResult CreateResponse<T>(IEnumerable<T> data, int pageSize, int pageNumber) => Ok(new PaginatedResponse<T>(data, pageSize, pageNumber));
}
