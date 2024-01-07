using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace MeuLivroDeReceitas.Api.WebSockets;

[Authorize(Policy = "UsuarioLogado")]
public class AdicionarConexao: Hub
{
    public async Task GetQRCode()
    {
        var qrCode = "ABCD123";

        await Clients.Caller.SendAsync("ResultadoQRCode", qrCode);
    }

    public override Task OnConnectedAsync()
    {

        return base.OnConnectedAsync();
    }
}
