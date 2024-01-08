using MeuLivroDeReceitas.Domain.Entidades;
using MeuLivroDeReceitas.Domain.Repositorios.Codigo;
using Microsoft.EntityFrameworkCore;

namespace MeuLivroDeReceitas.Infrastructure.AcessoRepositorio.Repositorio;
public class CodigoRepositorio: ICodigoWriteOnlyRepositorio
{
    private readonly MeuLivroDeReceitasContext _contexto;

    public CodigoRepositorio(MeuLivroDeReceitasContext contexto)
    {
        _contexto = contexto;
    }

    public async Task Registrar(Codigos codigo)
    {
        var codigoBancoDeDados = await _contexto.Codigos.FirstOrDefaultAsync(c => c.UsuarioId == codigo.UsuarioId);
        if (codigoBancoDeDados is not null)
        {
            codigoBancoDeDados.Codigo = codigo.Codigo;
            _contexto.Codigos.Update(codigoBancoDeDados);
        }
        else
        {
            await _contexto.Codigos.AddAsync(codigo);
        }
    }
}
