using Xunit;
using Microsoft.EntityFrameworkCore;
using DesafioTecnicoObjective.Models;
using DesafioTecnicoObjective.Services;
using DesafioTecnicoObjective.DTOs;
using DesafioTecnicoObjective.Repositories;
using DesafioTecnicoObjective.Exceptions;

namespace DesafioTecnicoObjective.DesafioTecnicoObjective.Test.Integration
{

    /// <summary>
    /// Testes de integração para o serviço de contas, validando persistência e regras de negócio com banco em memória.
    /// </summary>
    [Collection("ContaServiceIntegrationTests")]
    public class ContaServiceIntegrationTests
    {

        /// <summary>
        /// Cria uma instância do ContaService utilizando um banco de dados em memória para testes de integração.
        /// Garante isolamento total entre os testes usando um nome de banco único por teste.
        /// </summary>
        /// <param name="context">Retorna o contexto do banco de dados criado.</param>
        /// <returns>Instância de ContaService configurada para testes.</returns>
        private ContaService CriarServiceComDb(out ContaDbContext context)
        {
            // Gera um nome de banco único para cada chamada, garantindo isolamento total
            var dbName = $"ContaDb_{Guid.NewGuid()}_{DateTime.UtcNow.Ticks}";
            var options = new DbContextOptionsBuilder<ContaDbContext>()
                .UseInMemoryDatabase(databaseName: dbName)
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
            // Redireciona o Console para evitar ObjectDisposedException causada por Console.WriteLine em testes paralelos
            var originalOut = Console.Out;
            using var sw = new StringWriter();
            Console.SetOut(sw);

            try
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
            finally
            {
                // Restaura o Console.Out original
                Console.SetOut(originalOut);
            }
        }

        /// <summary>
        /// Testa se uma transação com saldo insuficiente lança exceção e não altera o saldo da conta no banco de dados.
        /// </summary>
        [Fact]
        public void RealizarTransacao_SaldoInsuficiente_LancaExcecaoENaoAtualiza()
        {
            // Redireciona o Console para evitar ObjectDisposedException causada por Console.WriteLine em testes paralelos
            var originalOut = Console.Out;
            using var sw = new StringWriter();
            Console.SetOut(sw);

            try
            {
                var service = CriarServiceComDb(out var context);
                var conta = new Conta { NumeroConta = 3, Saldo = 10 };
                context.Contas.Add(conta);
                context.SaveChanges();

                var dto = new TransacaoCreateDto { NumeroConta = 3, FormaPagamento = "C", Valor = 100 };

                var ex = Assert.Throws<SaldoInsuficienteException>(() => service.RealizarTransacao(dto));
                Assert.Equal("Saldo insuficiente.", ex.Message);

                var contaBanco = context.Contas.Find(3);
                Assert.NotNull(contaBanco);
                Assert.Equal(10, contaBanco!.Saldo);
            }
            finally
            {
                // Restaura o Console.Out original
                Console.SetOut(originalOut);
            }
        }

        /// <summary>
        /// Testa se o decorator LogTaxaStrategyDecorator está sendo utilizado durante a transação,
        /// verificando se as mensagens de log são escritas no console ao calcular a taxa.
        /// </summary>
        [Fact]
        public void RealizarTransacao_DeveLogarNoConsole()
        {
            var service = CriarServiceComDb(out var context);
            var conta = new Conta { NumeroConta = 10, Saldo = 100 };
            context.Contas.Add(conta);
            context.SaveChanges();

            var dto = new TransacaoCreateDto { NumeroConta = 10, FormaPagamento = "D", Valor = 10 };

            var originalOut = Console.Out;
            using var sw = new StringWriter();
            Console.SetOut(sw);

            try
            {
                service.RealizarTransacao(dto);

                var output = sw.ToString();
                Assert.Contains("Calculando taxa", output);
                Assert.Contains("Taxa calculada", output);
            }
            finally
            {
                Console.SetOut(originalOut);
            }
        }
        /// <summary>
        /// Testa se tentar realizar uma transação em uma conta inexistente lança exceção.
        /// </summary>
        [Fact]
        public void RealizarTransacao_ContaInexistente_DeveLancarExcecao()
        {
            var service = CriarServiceComDb(out _);
            var dto = new TransacaoCreateDto { NumeroConta = 999, FormaPagamento = "D", Valor = 10 };

            var ex = Assert.Throws<ContaNotFoundException>(() => service.RealizarTransacao(dto));
            Assert.Contains("Conta não encontrada.", ex.Message, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Testa se criar uma conta duplicada lança exceção.
        /// </summary>
        [Fact]
        public void CriarConta_Duplicada_DeveLancarExcecao()
        {
            var service = CriarServiceComDb(out _);
            var dto = new ContaCreateDto { NumeroConta = 1, Saldo = 100 };
            service.CriarConta(dto);

            var ex = Assert.Throws<ContaJaExisteException>(() => service.CriarConta(dto));
            Assert.Contains("Conta já existe.", ex.Message, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Testa se realizar uma transação com forma de pagamento inválida lança exceção.
        /// </summary>
        [Fact]
        public void RealizarTransacao_FormaPagamentoInvalida_DeveLancarExcecao()
        {
            var service = CriarServiceComDb(out var context);
            var conta = new Conta { NumeroConta = 5, Saldo = 100 };
            context.Contas.Add(conta);
            context.SaveChanges();

            var dto = new TransacaoCreateDto { NumeroConta = 5, FormaPagamento = "X", Valor = 10 };

            var ex = Assert.Throws<InvalidOperationException>(() => service.RealizarTransacao(dto));
            Assert.Contains("forma de pagamento", ex.Message, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Testa se obter uma conta inexistente lança exceção.
        /// </summary>
        [Fact]
        public void ObterConta_ContaInexistente_DeveLancarExcecao()
        {
            var service = CriarServiceComDb(out _);

            Assert.Throws<ContaNotFoundException>(() => service.ObterConta(999));
        }

    }
}