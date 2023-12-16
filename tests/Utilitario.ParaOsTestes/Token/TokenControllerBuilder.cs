using MeuLivroDeReceitas.Application.Servicos.Token;

namespace Utilitario.ParaOsTestes.Token;

public class TokenControllerBuilder
{
    public static TokenController Instancia()
    {
        return new TokenController(1000, "dX5hQ3xWOjleVTM5QnN6JWIpL0o4ITk1I2cqYyIjX3g6");
    }

    public static TokenController TokenExpirado()
    {
        return new TokenController(0.0166667, "dX5hQ3xWOjleVTM5QnN6JWIpL0o4ITk1I2cqYyIjX3g6");
    }
}
