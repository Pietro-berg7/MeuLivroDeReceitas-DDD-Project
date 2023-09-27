﻿using FluentMigrator.Runner;
using MeuLivroDeReceitas.Domain.Extension;
using MeuLivroDeReceitas.Domain.Repositorios;
using MeuLivroDeReceitas.Infrastructure.AcessoRepositorio;
using MeuLivroDeReceitas.Infrastructure.AcessoRepositorio.Repositorio;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using FluentMigrator.Runner.Initialization;
using Microsoft.EntityFrameworkCore;

namespace MeuLivroDeReceitas.Infrastructure;

public static class Bootstrapper
{
    public static void AddRepositorio(this IServiceCollection services, IConfiguration configurationManager)
    {
        AddFluentMigrator(services, configurationManager);

        AddContexto(services, configurationManager);
        AddUnidadeDeTrabalho(services);
        AddRepositorios(services);
    }

    private static void AddContexto(IServiceCollection services, IConfiguration configurationManager)
    {
        var connectionString = configurationManager.GetConexaoCompleta();

        services.AddDbContext<MeuLivroDeReceitasContext>(dbContextoOpcpes =>
        {
            dbContextoOpcpes.UseSqlServer(connectionString);
        });
    }

    private static void AddUnidadeDeTrabalho(IServiceCollection services)
    {
        services.AddScoped<IUnidadeDeTrabalho, UnidadeDeTrabalho>();
    }

    private static void AddRepositorios(IServiceCollection services)
    {
        services
            .AddScoped<IUsuarioWriteOnlyRepositorio, UsuarioRepositorio>()
            .AddScoped<IUsuarioReadOnlyRepositorio, UsuarioRepositorio>();
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
