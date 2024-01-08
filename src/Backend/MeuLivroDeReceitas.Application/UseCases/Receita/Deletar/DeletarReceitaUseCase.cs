﻿
using AutoMapper;
using MeuLivroDeReceitas.Application.Servicos.UsuarioLogado;
using MeuLivroDeReceitas.Comunicacao.Respostas;
using MeuLivroDeReceitas.Domain.Repositorios.Receita;
using MeuLivroDeReceitas.Exceptions.ExceptionsBase;
using MeuLivroDeReceitas.Exceptions;
using MeuLivroDeReceitas.Domain.Repositorios;

namespace MeuLivroDeReceitas.Application.UseCases.Receita.Deletar;
public class DeletarReceitaUseCase: IDeletarReceitaUseCase
{
    private readonly IReceitaWriteOnlyRepositorio _repositorioWriteOnly;
    private readonly IReceitaReadOnlyRepositorio _repositorioReadOnly;
    private readonly IUsuarioLogado _usuarioLogado;
    private readonly IUnidadeDeTrabalho _unidadeDeTrabalho;

    public DeletarReceitaUseCase(
        IReceitaWriteOnlyRepositorio repositorioWriteOnly, 
        IReceitaReadOnlyRepositorio repositorioReadOnly, 
        IUsuarioLogado usuarioLogado,
        IUnidadeDeTrabalho unidadeDeTrabalho)
    {
        _repositorioWriteOnly = repositorioWriteOnly;
        _repositorioReadOnly = repositorioReadOnly;
        _usuarioLogado = usuarioLogado;
        _unidadeDeTrabalho = unidadeDeTrabalho;
    }

    public async Task Executar(long id)
    {
        var usuarioLogado = await _usuarioLogado.RecuperarUsuario();

        var receita = await _repositorioReadOnly.RecuperarPorId(id);

        Validar(usuarioLogado, receita);

        await _repositorioWriteOnly.Deletar(id);

        await _unidadeDeTrabalho.Commit();
    }

    public static void Validar(Domain.Entidades.Usuario usuarioLogado, Domain.Entidades.Receita receita)
    {
        if (receita is null || receita.UsuarioId != usuarioLogado.Id)
        {
            throw new ErrosDeValidacaoException(new List<string> { ResourceMensagensDeErro.RECEITA_NAO_ENCONTRADA });
        }
    }
}
