using AutoMapper;
using MeuLivroDeReceitas.Application.Servicos.UsuarioLogado;
using MeuLivroDeReceitas.Comunicacao.Requisicoes;
using MeuLivroDeReceitas.Comunicacao.Respostas;
using MeuLivroDeReceitas.Domain.Extension;
using MeuLivroDeReceitas.Domain.Repositorios.Conexao;
using MeuLivroDeReceitas.Domain.Repositorios.Receita;
using System.Collections;

namespace MeuLivroDeReceitas.Application.UseCases.Dashboard;
public class DashboardUseCase: IDashboardUseCase
{
    private readonly IConexaoReadOnlyRepositorio _conexoesRepositorio;
    private readonly IReceitaReadOnlyRepositorio _repositorio;
    private readonly IUsuarioLogado _usuarioLogado;
    private readonly IMapper _mapper;

    public DashboardUseCase(
        IReceitaReadOnlyRepositorio repositorio,
        IUsuarioLogado usuarioLogado,
        IMapper mapper,
        IConexaoReadOnlyRepositorio conexoesRepositorio)
    {
        _repositorio = repositorio;
        _usuarioLogado = usuarioLogado;
        _mapper = mapper;
        _conexoesRepositorio = conexoesRepositorio;
    }

    public async Task<RespostaDashboardJson> Executar(RequisicaoDashboardJson requisicao)
    {
        var usuarioLogado = await _usuarioLogado.RecuperarUsuario();

        var receitas = await _repositorio.RecuperarTodasDoUsuario(usuarioLogado.Id);

        receitas = Filtrar(requisicao, receitas);

        var receitasUsuariosConectados = await ReceitasUsuariosConectados(requisicao, usuarioLogado);

        receitas = receitas.Concat(receitasUsuariosConectados).ToList();

        return new RespostaDashboardJson
        {
            Receitas = _mapper.Map<List<RespostaReceitaDashboardJson>>(receitas)
        };
    }

    private async Task<IList<Domain.Entidades.Receita>> ReceitasUsuariosConectados(RequisicaoDashboardJson requisicao, Domain.Entidades.Usuario usuarioLogado)
    {
        var conexoes = await _conexoesRepositorio.RecuperarDoUsuario(usuarioLogado.Id);

        var usuariosConectados = conexoes.Select(c => c.Id).ToList();
        var receitasUsuariosConectados = await _repositorio.RecuperarTodasDosUsuarios(usuariosConectados);

        return Filtrar(requisicao, receitasUsuariosConectados);
    }

    private static IList<Domain.Entidades.Receita> Filtrar(RequisicaoDashboardJson requisicao, IList<Domain.Entidades.Receita> receitas)
    {
        var receitasFiltradas = receitas;

        if (requisicao.Categoria.HasValue)
        {
            receitasFiltradas = receitas.Where(r => r.Categoria == (Domain.Enum.Categoria)requisicao.Categoria.Value).ToList();
        }

        if (!string.IsNullOrWhiteSpace(requisicao.TituloOuIngrediente))
        {
            receitasFiltradas = receitas
                .Where(r => r.Titulo.CompararSemConsiderarAcentoUpperCase(requisicao.TituloOuIngrediente) || r.Ingredientes.Any(ingrediente => ingrediente.Produto.CompararSemConsiderarAcentoUpperCase(requisicao.TituloOuIngrediente))).ToList();
        }

        return receitasFiltradas.OrderBy(c => c.Titulo).ToList();
    }
}
