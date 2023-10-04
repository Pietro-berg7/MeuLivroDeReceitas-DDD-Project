using MeuLivroDeReceitas.Comunicacao.Requisicoes;

namespace MeuLivroDeReceitas.Application.UseCases.Usuario.Registrar;

public class RegistrarUsuarioUseCase
{
    public Task Executar(RequisicaoRegistrarUsuarioJson requisicao)
    {

    }

    private void Validar(RequisicaoRegistrarUsuarioJson requisicao)
    {
        var validator = new RegistrarUsuarioValidator();
        var resultado = validator.Validate(requisicao);

        if (!resultado.IsValid)
        {
            var mensagensDeErro = resultado.Errors.Select(error => error.ErrorMessage);
            throw new Exception();
        }
    }
}
