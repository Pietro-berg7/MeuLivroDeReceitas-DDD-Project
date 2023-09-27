using FluentMigrator.Runner;
using MeuLivroDeReceitas.Domain.Extension;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace MeuLivroDeReceitas.Infrastructure;

public static class Bootstrapper
{
    public static void AddRepositorio(this IServiceCollection services, IConfiguration configurationManager)
    {
        AddFluentMigrator(services, configurationManager);
    }

    public static void AddFluentMigrator(IServiceCollection services, IConfiguration configurationManager)
    {
        services.AddFluentMigratorCore().ConfigureRunner(c =>
        {
            c.AddSqlServer()
                .WithGlobalConnectionString(configurationManager.GetConexaoCompleta())
                .ScanIn(Assembly.Load("MeuLivroDeReceitas.Infrastructure"))
                .For.All();
        });
    }
}
