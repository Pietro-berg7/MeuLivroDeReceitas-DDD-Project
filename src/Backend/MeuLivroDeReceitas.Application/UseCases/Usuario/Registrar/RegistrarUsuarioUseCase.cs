using AutoMapper;
using MeuLivroDeReceitas.Comunicacao.Requisicoes;
using MeuLivroDeReceitas.Domain.Repositorios;
using MeuLivroDeReceitas.Exceptions.ExceptionsBase;
using MeuLivroDeReceitas.Infrastructure.AcessoRepositorio.Repositorio;

namespace MeuLivroDeReceitas.Application.UseCases.Usuario.Registrar;

public class RegistrarUsuarioUseCase : IRegistrarUsuarioUseCase
{
    private readonly IUsuarioWriteOnlyRepositorio _repositorio;
    private readonly IMapper _mapper;
    private readonly IUnidadeDeTrabalho _unidadeDeTrabalho;

    public RegistrarUsuarioUseCase(IUsuarioWriteOnlyRepositorio repositorio, IMapper mapper, IUnidadeDeTrabalho unidadeDeTrabalho)
    {
        _repositorio = repositorio;
        _mapper = mapper;
        _unidadeDeTrabalho = unidadeDeTrabalho;
    }

    public async Task Executar(RequisicaoRegistrarUsuarioJson requisicao)
    {
        Validar(requisicao);

        var entidade = _mapper.Map<Domain.Entidades.Usuario>(requisicao);
        entidade.Senha = "cript";

        await _repositorio.Adicionar(entidade);

        await _unidadeDeTrabalho.Commit();
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
