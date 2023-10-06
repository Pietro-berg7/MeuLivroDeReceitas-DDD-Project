using MeuLivroDeReceitas.Application.UseCases.Usuario.Registrar;
using MeuLivroDeReceitas.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace MeuLivroDeReceitas.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController: ControllerBase
    {
        [HttpGet(Name = "GetWeatherForecast")]
        public async Task<IActionResult> Get()
        {
            var usecase = new RegistrarUsuarioUseCase();
            await usecase.Executar(new Comunicacao.Requisicoes.RequisicaoRegistrarUsuarioJson
            {

            });

            return Ok();
        }
    }
}