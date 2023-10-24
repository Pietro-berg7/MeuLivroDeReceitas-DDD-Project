using MeuLivroDeReceitas.Comunicacao.Requisicoes;

namespace MeuLivroDeReceitas.Application.UseCases.Usuario.Registrar;

public interface IRegistrarUsuarioUseCase
{
    Task Executar(RequisicaoRegistrarUsuarioJson requisicao);
}
