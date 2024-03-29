﻿using FluentAssertions;
using MeuLivroDeReceitas.Exceptions;
using System.Net;
using System.Text.Json;
using Utilitario.ParaOsTestes.Requisicoes;
using WebApi.Test.V1;
using Xunit;

namespace WebApi.Test.Usuario.Registrar;

public class RegistrarUsuarioTeste : ControllerBase
{
    private const string METODO = "usuario";

    public RegistrarUsuarioTeste(MeuLivroReceitaWebApplicationFactory<Program> factory) : base(factory)
    {

    }

    [Fact]
    public async Task Validar_Sucesso()
    {
        var requisicao = RequisicaoRegistrarUsuarioBuilder.Construir();

        var resposta = await PostRequest(METODO, requisicao);

        resposta.StatusCode.Should().Be(HttpStatusCode.Created);

        await using var responseBody = await resposta.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);

        responseData.RootElement.GetProperty("token").GetString().Should().NotBeNullOrWhiteSpace();
    }

    [Fact]
    public async Task Validar_Erro_Nome_Vazio()
    {
        var requisicao = RequisicaoRegistrarUsuarioBuilder.Construir();
        requisicao.Nome = "";

        var resposta = await PostRequest(METODO, requisicao);

        resposta.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        await using var responseBody = await resposta.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);

        var erros = responseData.RootElement.GetProperty("mensagens").EnumerateArray();
        erros.Should().ContainSingle().And.Contain(c => c.GetString().Equals(ResourceMensagensDeErro.NOME_USUARIO_EM_BRANCO));
    }
}
