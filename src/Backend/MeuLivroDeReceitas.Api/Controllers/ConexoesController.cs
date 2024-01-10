using MeuLivroDeReceitas.Api.Binder;
using MeuLivroDeReceitas.Api.Filtros.UsuarioLogado;
using MeuLivroDeReceitas.Application.UseCases.Conexao.Recuperar;
using MeuLivroDeReceitas.Application.UseCases.Conexao.Remover;
using MeuLivroDeReceitas.Comunicacao.Respostas;
using Microsoft.AspNetCore.Mvc;

namespace MeuLivroDeReceitas.Api.Controllers;

[ServiceFilter(typeof(UsuarioAutenticadoAttribute))]
public class ConexoesController: MeuLivroDeReceitasController
{
    [HttpGet]
    [ProducesResponseType(typeof(IList<RespostaUsuarioConectadoJson>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> RecuperarConexoes([FromServices] IRecuperarTodasConexoesUseCase useCase)
    {
        var resultado = await useCase.Executar();

        if (resultado.Usuarios.Any())
        {
            return Ok(resultado);
        }

        return NoContent();
    }

    [HttpDelete]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> RemoverConexoes(
        [FromServices] IRemoverConexaoUseCase useCase,
        [FromRoute][ModelBinder(typeof(HashidsModelBinder))] long id)
    {
        await useCase.Executar(id);

        return NoContent();
    }
}