﻿using FluentAssertions;
using MeuLivroDeReceitas.Application.UseCases.Usuario.AlterarSenha;
using MeuLivroDeReceitas.Comunicacao.Requisicoes;
using MeuLivroDeReceitas.Exceptions;
using MeuLivroDeReceitas.Exceptions.ExceptionsBase;
using Utilitario.ParaOsTestes.Criptografia;
using Utilitario.ParaOsTestes.Entidades;
using Utilitario.ParaOsTestes.Repositorios;
using Utilitario.ParaOsTestes.Requisicoes;
using Utilitario.ParaOsTestes.UsuarioLogado;
using Xunit;

namespace UseCases.Test.Usuario.AlterarSenha;

public class AlterarSenhaUseCaseTest
{
    [Fact]
    public async Task Validar_Sucesso()
    {
        (var usuario, var senha) = UsuarioBuilder.Construir();

        var useCase = CriarUseCase(usuario);

        var requisicao = RequisicaoAlterarSenhaUsuarioBuilder.Construir();
        requisicao.SenhaAtual = senha;

        Func<Task> acao = async () =>
        {
            await useCase.Executar(new RequisicaoAlterarSenhaJson
            {
                SenhaAtual = senha,
                NovaSenha = "@NovaSenha1234"
            });
        };

        await acao.Should().NotThrowAsync();
    }

    [Fact]
    public async Task Validar_Erro_NovaSenhaEmBranco()
    {
        (var usuario, var senha) = UsuarioBuilder.Construir();

        var useCase = CriarUseCase(usuario);

        Func<Task> acao = async () =>
        {
            await useCase.Executar(new RequisicaoAlterarSenhaJson
            {
                SenhaAtual = senha,
                NovaSenha = ""
            });
        };

        await acao.Should().ThrowAsync<ErrosDeValidacaoException>()
            .Where(ex => ex.MensagensDeErro.Count == 1 && ex.MensagensDeErro.Contains(ResourceMensagensDeErro.SENHA_USUARIO_EM_BRANCO));
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    [InlineData(4)]
    [InlineData(5)]
    public async Task Validar_Erro_SenhaAtual_Invalida(int tamanhoSenha)
    {
        (var usuario, var senha) = UsuarioBuilder.Construir();

        var useCase = CriarUseCase(usuario);

        var requisicao = RequisicaoAlterarSenhaUsuarioBuilder.Construir(tamanhoSenha);
        requisicao.SenhaAtual = senha;

        Func<Task> acao = async () =>
        {
            await useCase.Executar(requisicao);
        };

        await acao.Should().ThrowAsync<ErrosDeValidacaoException>()
            .Where(ex => ex.MensagensDeErro.Count == 1 && ex.MensagensDeErro.Contains(ResourceMensagensDeErro.SENHA_USUARIO_MINIMO_SEIS_CARACTERES));
    }

    private AlterarSenhaUseCase CriarUseCase(MeuLivroDeReceitas.Domain.Entidades.Usuario usuario)
    {
        var encriptador = EncriptadorDeSenhaBuilder.Instancia();
        var unidadeTrabalho = UnidadeDeTrabalhoBuilder.Instancia().Construir();
        var repositorio = UsuarioUpdateOnlyRepositorioBuilder.Instancia().RecuperarPorId(usuario).Construir();
        var usuarioLogado = UsuarioLogadoBuilder.Instancia().RecuperarUsuario(usuario).Construir();

        return new AlterarSenhaUseCase(usuarioLogado, repositorio, encriptador, unidadeTrabalho);
    }
}
