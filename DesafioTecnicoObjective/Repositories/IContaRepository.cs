using DesafioTecnicoObjective.Models;

namespace DesafioTecnicoObjective.Repositories
{

    /// <summary>
    /// Interface para o repositório de contas, responsável pelas operações de persistência e consulta de contas.
    /// </summary>
    public interface IContaRepository
    {
        /// <summary>
        /// Obtém uma conta pelo número da conta.
        /// </summary>
        /// <param name="numeroConta">Número identificador da conta.</param>
        /// <returns>Objeto <see cref="Conta"/> correspondente ao número informado, ou null se não encontrada.</returns>
        Conta? GetByNumero(int numeroConta);

        /// <summary>
        /// Adiciona uma nova conta ao repositório.
        /// </summary>
        /// <param name="conta">Objeto <see cref="Conta"/> a ser adicionado.</param>
        void Add(Conta conta);

        /// <summary>
        /// Atualiza uma conta existente no repositório.
        /// </summary>
        /// <param name="conta">Objeto <see cref="Conta"/> com os dados atualizados.</param>
        void Update(Conta conta);

        /// <summary>
        /// Verifica se uma conta existe no repositório pelo número da conta.
        /// </summary>
        /// <param name="numeroConta">Número identificador da conta.</param>
        /// <returns>True se a conta existir, caso contrário, false.</returns>
        bool Exists(int numeroConta);
    }
}