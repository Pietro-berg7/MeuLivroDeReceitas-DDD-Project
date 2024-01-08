namespace MeuLivroDeReceitas.Domain.Repositorios.Codigo;
public interface ICodigoWriteOnlyRepositorio
{
    Task Registrar(Entidades.Codigos codigos);
}
