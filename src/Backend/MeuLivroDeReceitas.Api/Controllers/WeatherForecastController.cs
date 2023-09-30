using MeuLivroDeReceitas.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace MeuLivroDeReceitas.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController: ControllerBase
    {
        [HttpGet(Name = "GetWeatherForecast")]
        public IActionResult Get()
        {
            ResourceMensagensDeErro.Culture = new System.Globalization.CultureInfo("pt");

            var mensagem = ResourceMensagensDeErro.EMAIL_USUARIO_EM_BRANCO;

            return Ok();
        }
    }
}