using DesafioTecnicoObjective.DTOs;
using DesafioTecnicoObjective.Models;
using DesafioTecnicoObjective.Repositories;
using System;

namespace DesafioTecnicoObjective.Services
{
    /// <summary>
    /// Serviço responsável pelas operações de negócio relacionadas a contas bancárias.
    /// </summary>
    public class ContaService : IContaService
    {
        private readonly IContaRepository _repo;

        /// <summary>
        /// Construtor que injeta o repositório de contas.
        /// </summary>
        /// <param name="repo">Repositório de contas utilizado pelo serviço.</param>
        public ContaService(IContaRepository repo)
        {
            _repo = repo;
        }

        /// <summary>
        /// Cria uma nova conta a partir dos dados informados.
        /// </summary>
        /// <param name="dto">DTO com os dados necessários para criação da conta.</param>
        /// <returns>Objeto <see cref="Conta"/> criado.</returns>
        /// <exception cref="InvalidOperationException">Lançada se a conta já existir.</exception>
        public Conta CriarConta(ContaCreateDto dto)
        {
            if (_repo.Exists(dto.NumeroConta))
                throw new InvalidOperationException("Conta já existe.");

            var conta = ContaFactory.CriarConta(dto);
            _repo.Add(conta);
            return conta;
        }

        /// <summary>
        /// Obtém as informações de uma conta pelo número da conta.
        /// </summary>
        /// <param name="numeroConta">Número identificador da conta.</param>
        /// <returns>DTO com as informações da conta, ou null se não encontrada.</returns>
        public ContaResponseDto? ObterConta(int numeroConta)
        {
            var conta = _repo.GetByNumero(numeroConta);
            if (conta == null) return null;
            return new ContaResponseDto
            {
                NumeroConta = conta.NumeroConta,
                Saldo = conta.Saldo
            };
        }

        /// <summary>
        /// Realiza uma transação (débito, crédito ou pix) em uma conta existente.
        /// </summary>
        /// <param name="dto">DTO com os dados da transação a ser realizada.</param>
        /// <returns>Objeto <see cref="Conta"/> atualizado após a transação, ou null se saldo insuficiente.</returns>
        /// <exception cref="InvalidOperationException">Lançada se a conta não for encontrada ou a forma de pagamento for inválida.</exception>
        public Conta? RealizarTransacao(TransacaoCreateDto dto)
        {
            var conta = _repo.GetByNumero(dto.NumeroConta);
            if (conta == null)
                throw new InvalidOperationException("Conta não encontrada.");

            var formaPagamento = dto.FormaPagamento.ToUpper();
            float taxa = CalcularTaxa(formaPagamento, dto.Valor);

            float total = dto.Valor + taxa;
            if (conta.Saldo < total)
                return null; // saldo insuficiente

            conta.Saldo -= total;
            _repo.Update(conta);
            return conta;
        }

        /// <summary>
        /// Calcula a taxa de acordo com a forma de pagamento e valor da transação.
        /// </summary>
        /// <param name="formaPagamento">Forma de pagamento (D, C, P).</param>
        /// <param name="valor">Valor da transação.</param>
        /// <returns>Valor da taxa calculada.</returns>
        /// <exception cref="InvalidOperationException">Lançada se a forma de pagamento for inválida.</exception>
        private float CalcularTaxa(string formaPagamento, float valor)
        {
            return formaPagamento switch
            {
                "D" => valor * 0.03f,
                "C" => valor * 0.05f,
                "P" => 0.0f,
                _ => throw new InvalidOperationException("Forma de pagamento inválida. Informe D (debito), C (credito) ou P (pix).")
            };
        }
    }

    /// <summary>
    /// Fábrica para criação de objetos Conta a partir de DTOs.
    /// </summary>
    public static class ContaFactory
    {
        /// <summary>
        /// Cria uma nova instância de Conta a partir de um DTO de criação de conta.
        /// </summary>
        /// <param name="dto">DTO com os dados necessários para criação da conta.</param>
        /// <returns>Nova instância de <see cref="Conta"/>.</returns>
        public static Conta CriarConta(ContaCreateDto dto)
        {
            return new Conta { NumeroConta = dto.NumeroConta, Saldo = dto.Saldo };
        }
    }
}
