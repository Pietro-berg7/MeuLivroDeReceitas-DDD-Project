using MeuLivroDeReceitas.Application.Servicos.Token;

namespace Utilitario.ParaOsTestes.Token;

public class TokenControllerBuilder
{
    public static TokenController Instancia()
    {
        return new TokenController(1000, "RyY5TXdqOGw9SXFKdEN5b0YnIl4yMSlQIVolaWNaMkx9NkA4JGdrWXUzO098bS81WmA=");
    }
}
