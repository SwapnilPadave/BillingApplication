using BA.Api.Infra.Model;
using BA.Service.CountryStateCity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BA.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CountryController : BaseController
    {
        private readonly ICountryStateCityService _countryStateCityService;
        public CountryController(ICountryStateCityService countryStateCityService)
        {
            _countryStateCityService = countryStateCityService;
        }
        [AllowAnonymous]
        [HttpPost("GetDetails")]
        public async Task<ResponseModel> Get(int? countryId, int? stateId)
        {
            var result = await _countryStateCityService.GetCountryStateCityDetails(countryId, stateId);
            if (result.IsSuccess)
            {
                return APISuccessResponse("BA100", result.Data!);
            }
            return APIFailureResponse(result.Error.ErrorMsg, null!);
        }
    }
}
