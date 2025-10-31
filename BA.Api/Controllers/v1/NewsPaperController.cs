using Asp.Versioning;
using BA.Api.Infra.MediatorHandlers.NewsPaperHandler;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BA.Api.Controllers.v1
{
    [ApiController]
    [Authorize]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class NewsPaperController : BaseController
    {
        private readonly IMediator _mediator;
        public NewsPaperController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("GetAll")]
        public async Task<Dictionary<string, object>> GetAllAsync(CancellationToken cancellationToken)
        {
            var data = await _mediator.Send(new GetNewsPaperDetailsQuery());
            if (data.IsSuccess)
                return APIResponse("BA100", data.Data!);
            return APIResponse(data.Error.ErrorMsg, null!);
        }

        [HttpPost("GetById")]
        public async Task<Dictionary<string, object>> GetByIdAsync([FromQuery] GetNewsPaperDetailsByIdQuery request)
        {
            var data = await _mediator.Send(request);
            if (data.IsSuccess)
                return APIResponse("BA100", data.Data!);
            return APIResponse(data.Error.ErrorMsg, null!);
        }

        [HttpPost("Add")]
        public async Task<Dictionary<string, object>> AddAsync([FromBody] CreateNewsPaperDetailsCommand command)
        {
            var result = await _mediator.Send(command);
            if (result.IsSuccess)
                return APIResponse("BA100", result.Data!);
            return APIResponse(result.Error.ErrorMsg, null!);
        }

        [HttpPost("Update")]
        public async Task<Dictionary<string, object>> UpdateAsync([FromBody] UpdateNewsPaperDetailsByIdCommand request, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(request);
            if (result.IsSuccess)
                return APIResponse("BA100", result.Data!);
            return APIResponse(result.Error.ErrorMsg, null!);
        }

        [HttpPost("Delete")]
        public async Task<Dictionary<string, object>> DeleteAsync([FromQuery] DeleteNewsPaperDetailsByIdQuery request, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(request);
            if (result.IsSuccess)
                return APIResponse("BA100", result.Data!);
            return APIResponse(result.Error.ErrorMsg, null!);
        }

        [HttpPost("ActivateOrDeactivate")]
        public async Task<Dictionary<string, object>> ActivateOrDeactivateAsync([FromQuery] ActivateAndDeactivateNewsPaperStatusByIdQuery request, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(request);
            if (result.IsSuccess)
                return APIResponse("BA100", result.Data!);
            return APIResponse(result.Error.ErrorMsg, null!);
        }
    }
}
