using AutoMapper;
using BA.Api.Infra.Requests.UserRequests;
using BA.Dtos.UserDtos;
using BA.Service.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BA.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController : BaseController
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        public UserController(IUserService userService
            , IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }

        [HttpPost("Add")]
        public async Task<Dictionary<string, object>> AddAsync([FromBody] AddUserRequest request, CancellationToken cancellationToken)
        {
            if(!ModelState.IsValid)
            {
                return APIResponse("BA101", ModelState);
            }

            var user = _mapper.Map<AddUserDto>(request);
            var result = await _userService.AddUserAsync(UserId, user, cancellationToken);
            if (result.IsSuccess)
                return APIResponse("BA100", result.Data!);
            return APIResponse(result.Error.ErrorMsg, null!);
        }

        [HttpPost("GetAll")]
        public async Task<Dictionary<string, object>> GetAsync(CancellationToken cancellationToken)
        {
            var result = await _userService.GetUsersAsync(cancellationToken);
            if (result.IsSuccess)
                return APIResponse("BA100", result.Data!);
            return APIResponse(result.Error.ErrorMsg, null!);
        }

        [HttpPost("GetById")]
        public async Task<Dictionary<string, object>> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            var result = await _userService.GetUserByIdAsync(id, cancellationToken);
            if (result.IsSuccess)
                return APIResponse("BA100", result.Data!);
            return APIResponse(result.Error.ErrorMsg, null!);
        }

        [HttpPost("Update")]
        public async Task<Dictionary<string, object>> UpdateAsync(int id, [FromBody] UpdateUserRequest request, CancellationToken cancellationToken)
        {
            var user = _mapper.Map<UpdateUserDto>(request);
            var result = await _userService.UpdateUserAsync(UserId, id, user, cancellationToken);
            if (result.IsSuccess)
                return APIResponse("BA100", result.Data!);
            return APIResponse(result.Error.ErrorMsg, null!);
        }

        [HttpDelete("Delete")]
        public async Task<Dictionary<string, object>> DeleteAsync(int id, CancellationToken cancellationToken)
        {
            var result = await _userService.DeleteUserAsync(UserId, id, cancellationToken);
            if (result.IsSuccess)
                return APIResponse("BA100", result.Data!);
            return APIResponse(result.Error.ErrorMsg, null!);
        }
    }
}
