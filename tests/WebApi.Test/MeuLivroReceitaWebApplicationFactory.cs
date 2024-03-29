﻿using MeuLivroDeReceitas.Infrastructure.AcessoRepositorio;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace WebApi.Test;

public class MeuLivroReceitaWebApplicationFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
{
    private MeuLivroDeReceitas.Domain.Entidades.Usuario _usuario;
    private string _senha;

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Test")
            .ConfigureServices(services =>
            {
                var descritor = services.SingleOrDefault(d => d.ServiceType == typeof(MeuLivroDeReceitasContext));
                if(descritor is not null)
                {
                    services.Remove(descritor);
                }

                var provider = services.AddEntityFrameworkInMemoryDatabase().BuildServiceProvider();

                services.AddDbContext<MeuLivroDeReceitasContext>(options =>
                {
                    options.UseInMemoryDatabase("InMemoryDbForTesting");
                    options.UseInternalServiceProvider(provider);
                });

                var serviceProvider = services.BuildServiceProvider();

                using var scope = serviceProvider.CreateScope();
                var scopeService = scope.ServiceProvider;

                var database = scopeService.GetRequiredService<MeuLivroDeReceitasContext>();

                database.Database.EnsureDeleted();

                (_usuario, _senha) = ContextSeedInMemory.Seed(database);
            });
    }

    public MeuLivroDeReceitas.Domain.Entidades.Usuario RecuperarUsuario()
    {
        return _usuario;
    }

    public string RecuperarSenha()
    {
        return _senha;
    }
}
