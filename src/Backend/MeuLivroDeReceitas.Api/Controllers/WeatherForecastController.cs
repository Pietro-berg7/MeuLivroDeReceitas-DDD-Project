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
        public async Task<IActionResult> Get([FromServices] IRegistrarUsuarioUseCase useCase)
        {
            await useCase.Executar(new Comunicacao.Requisicoes.RequisicaoRegistrarUsuarioJson
            {
                Email = "betotle@gmail.com",
                Nome = "Betotle",
                Senha = "123456",
                Telefone = "37 9 1234-5678"
            }); ;

            return Ok();
        }
    }
}