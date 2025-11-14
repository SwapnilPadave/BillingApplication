using Asp.Versioning;
using BA.Api.Infra.MediatorHandlers.AuthHandler;
using BA.Api.Infra.Model;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BA.Api.Controllers.v1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class AuthController : BaseController
    {
        private readonly IMediator _mediator;
        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("Login")]
        public async Task<ResponseModel> Login([FromBody] AuthenticationQuery request, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(request);

            if (result.IsSuccess)
                return APISuccessResponse("BA100", result.Data!);
            return APIFailureResponse(result.Error.ErrorMsg, null!);
        }

        [HttpPost("RefreshToken")]
        public async Task<ResponseModel> Refresh([FromBody] RefreshTokenCommand dto)
        {
            var result = await _mediator.Send(dto);
            if (!result.IsSuccess)
            {
                return APIFailureResponse(result.Error.ErrorMsg, null!);
            }
            return APISuccessResponse("BA100", result.Data!);
        }
    }
}
