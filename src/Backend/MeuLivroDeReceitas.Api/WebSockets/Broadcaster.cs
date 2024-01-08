using MeuLivroDeReceitas.Exceptions;
using MeuLivroDeReceitas.Exceptions.ExceptionsBase;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Concurrent;

namespace MeuLivroDeReceitas.Api.WebSockets;

public class Broadcaster
{
    private readonly static Lazy<Broadcaster> _instance = new(() => new Broadcaster());

    public static Broadcaster Instance { get { return _instance.Value; } }

    private ConcurrentDictionary<string, object> Dictionary { get; set; }

    public Broadcaster()
    {
        Dictionary = new ConcurrentDictionary<string, object>();
    }

    public void InicializarConexao(IHubContext<AdicionarConexao> hubContext, string idUsuarioQueGerouQrCode, string connectionId)
    {
        var conexao = new Conexao(hubContext, connectionId);

        Dictionary.TryAdd(connectionId, conexao);
        Dictionary.TryAdd(idUsuarioQueGerouQrCode, connectionId);

        conexao.IniciarContagemTempo(CallbackTempoExpirado);
    }

    private void CallbackTempoExpirado(string connectionId)
    {
        Dictionary.TryRemove(connectionId, out _);
    }

    public string GetConnectionIdDoUsuario(string usuarioId)
    {
        if (!Dictionary.TryGetValue(usuarioId, out var connectionId))
        {
            throw new MeuLivroDeReceitasException(ResourceMensagensDeErro.USUARIO_NAO_ENCONTRADO);
        }

        return connectionId.ToString();
    }

    public void ResetarTempoExpiracao(string connectionId)
    {
        Dictionary.TryGetValue(connectionId, out var objetoConexao);

        var conexao = objetoConexao as Conexao;

        conexao.ResetarContagemTempo();
    }

    public void SetConnectionIdUsuarioLeitorQRCode(string idUsuarioQueGerouQRCode, string connectionIdUsarioLeitorQRCode)
    {
        var connectionIdUsuarioQueLeuQRCode = GetConnectionIdDoUsuario(idUsuarioQueGerouQRCode);

        Dictionary.TryGetValue(connectionIdUsuarioQueLeuQRCode, out var objetoConexao);

        var conexao = objetoConexao as Conexao;

        conexao.SetConnectionIdUsuarioLeitorQRCode(connectionIdUsarioLeitorQRCode);
    }

    public string Remover(string connectionId, string usuarioId)
    {
        if (!Dictionary.TryGetValue(connectionId, out var objetoConexao))
        {
            throw new MeuLivroDeReceitasException(ResourceMensagensDeErro.USUARIO_NAO_ENCONTRADO);
        }

        var conexao = objetoConexao as Conexao;

        conexao.StopTimer();

        Dictionary.TryRemove(connectionId, out _);
        Dictionary.TryRemove(usuarioId, out _);

        return conexao.UsuarioQueLeuQRCode();
    }
}