using Microsoft.AspNetCore.Mvc;

namespace AIWorksService.Controllers
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
        private readonly IConfiguration _configuration;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public async Task<IEnumerable<WeatherForecast>> Get()
        {
            string? result = string.Empty;
            string? url = string.Empty;
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    url = _configuration["WORKS_API"];
                    if (url != null)
                    {
                        var response = await client.GetAsync(url);
                        if (response != null)
                        {
                            result = await response.Content.ReadAsStringAsync();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                    result = ex.StackTrace;
            }
            result = result ?? "The result is null";
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)],
                Greeting = result,
                Url = url
            })
            .ToArray();
        }

        [HttpGet("/ping")]
        public IActionResult Ping()
        {
            return Ok();
        }

        [HttpGet("/get")]
        public IActionResult GetAI()
        {
            string? result = string.Empty;
            string? url = string.Empty;
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    url = _configuration["WORKS_API"];
                    if (url != null)
                    {
                        var response = client.GetAsync(url).Result;
                        if (response != null)
                        {
                            result = response.Content.ReadAsStringAsync().Result;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                result = ex.StackTrace;
            }
            result = result ?? "The result is null";

            return Ok(result);
        }
    }
}
