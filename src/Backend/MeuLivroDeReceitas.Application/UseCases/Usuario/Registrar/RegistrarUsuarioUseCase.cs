using MeuLivroDeReceitas.Comunicacao.Requisicoes;
using MeuLivroDeReceitas.Exceptions.ExceptionsBase;

namespace MeuLivroDeReceitas.Application.UseCases.Usuario.Registrar;

public class RegistrarUsuarioUseCase
{
    public async Task Executar(RequisicaoRegistrarUsuarioJson requisicao)
    {
        Validar(requisicao); 
        
        //salvar no banco de dados
    }

    private void Validar(RequisicaoRegistrarUsuarioJson requisicao)
    {
        var validator = new RegistrarUsuarioValidator();
        var resultado = validator.Validate(requisicao);

        if (!resultado.IsValid)
        {
            var mensagensDeErro = resultado.Errors.Select(error => error.ErrorMessage).ToList();
            throw new ErrosDeValidacaoException(mensagensDeErro);
        }
    }
}
