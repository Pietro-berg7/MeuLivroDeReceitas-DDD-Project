namespace MeuLivroDeReceitas.Domain.Repositorios;

public interface IUsuarioWriteOnlyRepositorio
{
    Task Adicionar(Entidades.Usuario usuario);
}
