using MeuLivroDeReceitas.Comunicacao.Enum;

namespace MeuLivroDeReceitas.Comunicacao.Requisicoes;
public class RequisicaoRegistrarReceitaJson
{
    public string Titulo { get; set; }
    public Categoria Categoria { get; set; }
    public string ModoPreparo { get; set; }
    public List<RequisicaoRegistrarIngredienteJson> Ingredientes { get; set; } = new();
}
