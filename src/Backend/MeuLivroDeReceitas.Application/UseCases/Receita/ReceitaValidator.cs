﻿using FluentValidation;
using MeuLivroDeReceitas.Comunicacao.Requisicoes;
using MeuLivroDeReceitas.Exceptions;

namespace MeuLivroDeReceitas.Application.UseCases.Receita;
public class ReceitaValidator: AbstractValidator<RequisicaoReceitaJson>
{
    public ReceitaValidator()
    {
        RuleFor(x => x.Titulo).NotEmpty().WithMessage(ResourceMensagensDeErro.TITULO_RECEITA_EM_BRANCO);
        RuleFor(x => x.Categoria).IsInEnum().WithMessage(ResourceMensagensDeErro.CATEGORIA_RECEITA_INVALIDA);
        RuleFor(x => x.ModoPreparo).NotEmpty().WithMessage(ResourceMensagensDeErro.MODOPREPARO_RECEITA_EM_BRANCO);
        RuleFor(x => x.Ingredientes).NotEmpty().WithMessage(ResourceMensagensDeErro.RECEITA_MINIMO_UM_INGREDIENTE);
        RuleForEach(x => x.Ingredientes).ChildRules(ingrediente =>
        {
            ingrediente.RuleFor(x => x.Produto).NotEmpty().WithMessage(ResourceMensagensDeErro.RECEITA_INGREDIENTE_PRODUTO_EM_BRANCO);
            ingrediente.RuleFor(x => x.Quantidade).NotEmpty().WithMessage(ResourceMensagensDeErro.RECEITA_INGREDIENTE_QUANTIDADE_EM_BRANCO);
        });

        RuleFor(x => x.Ingredientes).Custom((ingredientes, contexto) =>
        {
            var produtosDistintos = ingredientes.Select(c => c.Produto).Distinct();
            if (produtosDistintos.Count() != ingredientes.Count)
            {
                contexto.AddFailure(new FluentValidation.Results.ValidationFailure("Ingredientes", ResourceMensagensDeErro.RECEITA_INGREDIENTES_REPETIDOS));
            }
        });
    }
}