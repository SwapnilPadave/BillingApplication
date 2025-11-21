using Microsoft.AspNetCore.Mvc;

namespace BA.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }

        #region different ways to call async method into sync method.
        //[HttpGet("Test")]
        //public async Task<ResponseModel> Test()
        //{
        //    int id = 0;
        //    var result = _billService.GetCustomerBillById(id).GetAwaiter().GetResult();

        //    var result1 = _billService.GetCustomerBillById(id).Result;

        //    _billService.GetCustomerBillById(id).Wait();
        //    Task.WaitAll();

        //    var result2 = Task.Run(() => _billService.GetCustomerBillById(id)).GetAwaiter().GetResult();

        //    return APISuccessResponse("", result);
        //}
        #endregion
    }
}
