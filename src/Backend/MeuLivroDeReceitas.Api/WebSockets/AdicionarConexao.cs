using MeuLivroDeReceitas.Application.UseCases.Conexao.GerarQRCode;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace MeuLivroDeReceitas.Api.WebSockets;

[Authorize(Policy = "UsuarioLogado")]
public class AdicionarConexao: Hub
{
    private readonly IGerarQRCodeUseCase _gerarQRCodeUseCase;

    public AdicionarConexao(IGerarQRCodeUseCase gerarQRCodeUseCase)
    {
        _gerarQRCodeUseCase = gerarQRCodeUseCase;
    }

    public async Task GetQRCode()
    {
        var qrCode = await _gerarQRCodeUseCase.Executar();

        await Clients.Caller.SendAsync("ResultadoQRCode", qrCode);
    }
}
