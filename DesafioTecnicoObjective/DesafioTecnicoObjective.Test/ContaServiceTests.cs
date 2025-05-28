using Xunit;
using Moq;
using DesafioTecnicoObjective.Repositories;
using DesafioTecnicoObjective.Services;
using DesafioTecnicoObjective.Models;
using DesafioTecnicoObjective.DTOs;
using DesafioTecnicoObjective.Exceptions;

namespace DesafioTecnicoObjective.Tests
{
    /// <summary>
    /// Testes unitários para a classe ContaService, cobrindo cenários de criação de conta e transações.
    /// </summary>
    public class ContaServiceTests
    {
        private readonly ContaService _service;
        private readonly Mock<IContaRepository> _repoMock;

        /// <summary>
        /// Inicializa o mock do repositório e a instância do serviço para os testes.
        /// </summary>
        public ContaServiceTests()
        {
            _repoMock = new Mock<IContaRepository>();
            _service = new ContaService(_repoMock.Object);
        }

        /// <summary>
        /// Testa se a criação de uma conta ocorre com sucesso quando a conta não existe.
        /// </summary>
        [Fact]
        public void CriarConta_DeveCriarComSucesso()
        {
            var dto = new ContaCreateDto { NumeroConta = 123, Saldo = 100 };
            _repoMock.Setup(r => r.Exists(dto.NumeroConta)).Returns(false);

            var conta = _service.CriarConta(dto);

            Assert.Equal(dto.NumeroConta, conta.NumeroConta);
            Assert.Equal(dto.Saldo, conta.Saldo);
            _repoMock.Verify(r => r.Add(It.IsAny<Conta>()), Times.Once);
        }

        /// <summary>
        /// Testa se a criação de uma conta lança exceção quando a conta já existe.
        /// </summary>
        [Fact]
        public void CriarConta_DeveFalharSeContaJaExiste()
        {
            var dto = new ContaCreateDto { NumeroConta = 123, Saldo = 100 };
            _repoMock.Setup(r => r.Exists(dto.NumeroConta)).Returns(true);

            Assert.Throws<InvalidOperationException>(() => _service.CriarConta(dto));
        }

        /// <summary>
        /// Testa se a realização de uma transação válida atualiza corretamente o saldo da conta.
        /// </summary>
        /// <param name="forma">Forma de pagamento (D, C, P).</param>
        /// <param name="saldoInicial">Saldo inicial da conta.</param>
        /// <param name="valor">Valor da transação.</param>
        /// <param name="saldoEsperado">Saldo esperado após a transação.</param>
        [Theory]
        [InlineData("D", 100, 10, 89.70)] // débito: 10 + 3% = 10.3
        [InlineData("C", 100, 10, 89.50)] // crédito: 10 + 5% = 10.5
        [InlineData("P", 100, 10, 90)]   // pix: sem taxa
        public void RealizarTransacao_Valida_DeveAtualizarSaldo(string forma, float saldoInicial, float valor, float saldoEsperado)
        {
            var conta = new Conta { NumeroConta = 123, Saldo = saldoInicial };
            _repoMock.Setup(r => r.GetByNumero(123)).Returns(conta);

            var dto = new TransacaoCreateDto { NumeroConta = 123, FormaPagamento = forma, Valor = valor };
            var result = _service.RealizarTransacao(dto);

            Assert.NotNull(result);
            Assert.Equal(Math.Round(saldoEsperado, 2), Math.Round(result.Saldo, 2));
            _repoMock.Verify(r => r.Update(It.IsAny<Conta>()), Times.Once);
        }

        /// <summary>
        /// Testa se uma transação com saldo insuficiente retorna null e não atualiza a conta.
        /// </summary>
        [Fact]
        public void RealizarTransacao_SaldoInsuficiente_DeveLancarExcecao()
        {
            // Arrange
            var dto = new TransacaoCreateDto("D", 123, 1000f); // valor maior que o saldo
            var repoMock = new Mock<IContaRepository>();
            repoMock.Setup(r => r.GetByNumero(123)).Returns(new Conta { NumeroConta = 123, Saldo = 100f });
            var service = new ContaService(repoMock.Object);

            // Act & Assert
            Assert.Throws<SaldoInsuficienteException>(() => service.RealizarTransacao(dto));
        }

        /// <summary>
        /// Testa se uma transação com forma de pagamento inválida lança exceção.
        /// </summary>
        [Fact]
        public void RealizarTransacao_FormaPagamentoInvalida_DeveLancarExcecao()
        {
            var conta = new Conta { NumeroConta = 123, Saldo = 100 };
            _repoMock.Setup(r => r.GetByNumero(123)).Returns(conta);

            var dto = new TransacaoCreateDto { NumeroConta = 123, FormaPagamento = "x", Valor = 10 };

            Assert.Throws<InvalidOperationException>(() => _service.RealizarTransacao(dto));
        }

        /// <summary>
        /// Testa se a criação de uma conta com saldo zero ocorre com sucesso.
        /// </summary>
        [Fact]
        public void CriarConta_ComSaldoZero_DeveCriarComSucesso()
        {
            var dto = new ContaCreateDto { NumeroConta = 456, Saldo = 0 };
            _repoMock.Setup(r => r.Exists(dto.NumeroConta)).Returns(false);

            var conta = _service.CriarConta(dto);

            Assert.Equal(dto.NumeroConta, conta.NumeroConta);
            Assert.Equal(dto.Saldo, conta.Saldo);
            _repoMock.Verify(r => r.Add(It.IsAny<Conta>()), Times.Once);
        }

        /// <summary>
        /// Testa se uma transação com valor zero não altera o saldo da conta.
        /// </summary>
        [Fact]
        public void RealizarTransacao_ValorZero_DeveAtualizarSemTaxa()
        {
            var conta = new Conta { NumeroConta = 789, Saldo = 100 };
            _repoMock.Setup(r => r.GetByNumero(789)).Returns(conta);

            var dto = new TransacaoCreateDto { NumeroConta = 789, FormaPagamento = "P", Valor = 0 };
            var result = _service.RealizarTransacao(dto);

            Assert.NotNull(result);
            Assert.Equal(100, result.Saldo);
            _repoMock.Verify(r => r.Update(It.IsAny<Conta>()), Times.Once);
        }

        /// <summary>
        /// Testa se uma transação onde o saldo é exatamente igual ao valor mais a taxa é permitida.
        /// </summary>
        [Fact]
        public void RealizarTransacao_SaldoExatamenteIgualAoTotal_DevePermitir()
        {
            var valor = 50f;
            var taxa = valor * 0.05f;
            var saldo = valor + taxa;
            var conta = new Conta { NumeroConta = 321, Saldo = saldo };
            _repoMock.Setup(r => r.GetByNumero(321)).Returns(conta);

            var dto = new TransacaoCreateDto { NumeroConta = 321, FormaPagamento = "C", Valor = valor };
            var result = _service.RealizarTransacao(dto);

            Assert.NotNull(result);
            Assert.Equal(0, result.Saldo, 2);
            _repoMock.Verify(r => r.Update(It.IsAny<Conta>()), Times.Once);
        }

        /// <summary>
        /// Testa se o método ObterConta retorna a conta correta.
        /// </summary>
        [Fact]
        public void ObterConta_DeveRetornarContaCorreta()
        {
            var conta = new Conta { NumeroConta = 654, Saldo = 200 };
            _repoMock.Setup(r => r.GetByNumero(654)).Returns(conta);

            var result = _service.ObterConta(654);

            Assert.NotNull(result);
            Assert.Equal(654, result.NumeroConta);
            Assert.Equal(200, result.Saldo);
        }

        /// <summary>
        /// Testa se uma transação não permite que o saldo da conta fique negativo.
        /// </summary>
        [Fact]
        public void RealizarTransacao_NaoPermiteSaldoNegativo()
        {
            var conta = new Conta { NumeroConta = 987, Saldo = 5 };
            _repoMock.Setup(r => r.GetByNumero(987)).Returns(conta);

            var dto = new TransacaoCreateDto { NumeroConta = 987, FormaPagamento = "C", Valor = 10 };

            Assert.Throws<SaldoInsuficienteException>(() => _service.RealizarTransacao(dto));
            _repoMock.Verify(r => r.Update(It.IsAny<Conta>()), Times.Never);
        }

        [Fact]
        public void GetStrategy_DeveLancarExcecao_ParaFormaPagamentoInvalida()
        {
            Assert.Throws<InvalidOperationException>(() => TaxaStrategyFactory.GetStrategy("X"));
        }

        [Fact]
        public void DebitoStrategy_DeveCalcularTaxaCorretamente()
        {
            var strategy = new DebitoStrategy();
            var taxa = strategy.CalcularTaxa(100);
            Assert.Equal(3, taxa); 
        }

        [Fact]
        public void CalcularTaxa_DeveChamarEstrategiaInterna()
        {
            // Arrange
            var mockStrategy = new Mock<ITaxaStrategy>();
            mockStrategy.Setup(s => s.CalcularTaxa(100)).Returns(10);

            var decorator = new LogTaxaStrategyDecorator(mockStrategy.Object);

            // Act
            var taxa = decorator.CalcularTaxa(100);

            // Assert
            Assert.Equal(10, taxa);
            mockStrategy.Verify(s => s.CalcularTaxa(100), Times.Once);
        }

        [Fact]
        public void CalcularTaxa_DeveLogarNoConsole()
        {
            var mockStrategy = new Mock<ITaxaStrategy>();
            mockStrategy.Setup(s => s.CalcularTaxa(It.IsAny<float>())).Returns(5);

            var decorator = new LogTaxaStrategyDecorator(mockStrategy.Object);

            using var sw = new StringWriter();
            Console.SetOut(sw);

            decorator.CalcularTaxa(50);

            var output = sw.ToString();
            Assert.Contains("Calculando taxa", output);
            Assert.Contains("Taxa calculada", output);
        }
    }
}
