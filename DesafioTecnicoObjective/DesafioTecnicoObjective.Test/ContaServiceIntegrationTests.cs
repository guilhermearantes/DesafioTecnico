using Xunit;
using Microsoft.EntityFrameworkCore;
using DesafioTecnicoObjective.Models;
using DesafioTecnicoObjective.Services;
using DesafioTecnicoObjective.DTOs;
using DesafioTecnicoObjective.Repositories;

/// <summary>
/// Testes de integração para o serviço de contas, validando persistência e regras de negócio com banco em memória.
/// </summary>
public class ContaServiceIntegrationTests
{

    /// <summary>
    /// Cria uma instância do ContaService utilizando um banco de dados em memória para testes de integração.
    /// </summary>
    /// <param name="context">Retorna o contexto do banco de dados criado.</param>
    /// <returns>Instância de ContaService configurada para testes.</returns>
    private ContaService CriarServiceComDb(out ContaDbContext context)
    {
        var options = new DbContextOptionsBuilder<ContaDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        context = new ContaDbContext(options);
        var repo = new ContaRepository(context);

        return new ContaService(repo);
    }

    /// <summary>
    /// Testa se a criação de uma conta persiste corretamente no banco de dados.
    /// </summary>
    [Fact]
    public void CriarConta_PersistenciaReal_DeveSalvarNoBanco()
    {
        var service = CriarServiceComDb(out var context);
        var dto = new ContaCreateDto { NumeroConta = 1, Saldo = 100 };
        service.CriarConta(dto);

        var contaSalva = context.Contas.Find(1);
        Assert.NotNull(contaSalva);
        Assert.Equal(100, contaSalva.Saldo);
    }


    /// <summary>
    /// Testa se a realização de uma transação atualiza o saldo da conta no banco de dados.
    /// </summary>
    [Fact]
    public void RealizarTransacao_AtualizaSaldoNoBanco()
    {
        var service = CriarServiceComDb(out var context);
        var conta = new Conta { NumeroConta = 2, Saldo = 200 };
        context.Contas.Add(conta);
        context.SaveChanges();

        var dto = new TransacaoCreateDto { NumeroConta = 2, FormaPagamento = "D", Valor = 50 };
        service.RealizarTransacao(dto);

        var contaAtualizada = context.Contas.Find(2);
        Assert.NotNull(contaAtualizada);
        Assert.True(contaAtualizada.Saldo < 200); // saldo diminuiu
    }

    /// <summary>
    /// Testa se uma transação com saldo insuficiente não altera o saldo da conta no banco de dados.
    /// </summary>
    [Fact]
    public void RealizarTransacao_SaldoInsuficiente_NaoAtualiza()
    {
        var service = CriarServiceComDb(out var context);
        var conta = new Conta { NumeroConta = 3, Saldo = 10 };
        context.Contas.Add(conta);
        context.SaveChanges();

        var dto = new TransacaoCreateDto { NumeroConta = 3, FormaPagamento = "C", Valor = 100 };
        var result = service.RealizarTransacao(dto);

        Assert.Null(result);
        var contaBanco = context.Contas.Find(3);
        Assert.NotNull(contaBanco); // Adicionado para garantir que contaBanco não seja nulo
        Assert.Equal(10, contaBanco!.Saldo); // saldo não mudou
    }
}
