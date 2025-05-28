using DesafioTecnicoObjective.DTOs;
using DesafioTecnicoObjective.Models;

namespace DesafioTecnicoObjective.Services
{
    /// <summary>
    /// Interface para o serviço de contas, responsável pelas operações de negócio relacionadas a contas bancárias.
    /// </summary>
    public interface IContaService
    {
        /// <summary>
        /// Cria uma nova conta a partir dos dados informados.
        /// </summary>
        /// <param name="dto">DTO com os dados necessários para criação da conta.</param>
        /// <returns>Objeto <see cref="Conta"/> criado.</returns>
        Conta CriarConta(ContaCreateDto dto);

        /// <summary>
        /// Obtém as informações de uma conta pelo número da conta.
        /// </summary>
        /// <param name="numeroConta">Número identificador da conta.</param>
        /// <returns>DTO com as informações da conta, ou null se não encontrada.</returns>
        ContaResponseDto? ObterConta(int numeroConta);

        /// <summary>
        /// Realiza uma transação (débito, crédito ou pix) em uma conta existente.
        /// </summary>
        /// <param name="dto">DTO com os dados da transação a ser realizada.</param>
        /// <returns>Objeto <see cref="Conta"/> atualizado após a transação, ou null se saldo insuficiente.</returns>
        Conta? RealizarTransacao(TransacaoCreateDto dto);
    }
}