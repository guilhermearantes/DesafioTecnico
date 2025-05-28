using DesafioTecnicoObjective.Models;

namespace DesafioTecnicoObjective.Repositories
{
    /// <summary>
    /// Implementação do repositório de contas utilizando Entity Framework.
    /// </summary>
    public class ContaRepository : IContaRepository
    {
        private readonly ContaDbContext _context;

        /// <summary>
        /// Construtor que recebe o contexto do banco de dados.
        /// </summary>
        /// <param name="context">Instância do contexto de banco de dados.</param>
        public ContaRepository(ContaDbContext context) => _context = context;

        /// <summary>
        /// Obtém uma conta pelo número da conta.
        /// </summary>
        /// <param name="numeroConta">Número da conta a ser buscada.</param>
        /// <returns>Objeto Conta correspondente ao número informado.</returns>
        /// <exception cref="InvalidOperationException">Lançada se a conta não for encontrada.</exception>
        public Conta? GetByNumero(int numeroConta)
        {
            return _context.Contas.Find(numeroConta); // Não lança exceção, apenas retorna null
        }

        /// <summary>
        /// Adiciona uma nova conta ao banco de dados.
        /// </summary>
        /// <param name="conta">Objeto Conta a ser adicionado.</param>
        public void Add(Conta conta) { _context.Contas.Add(conta); _context.SaveChanges(); }

        /// <summary>
        /// Atualiza uma conta existente no banco de dados.
        /// </summary>
        /// <param name="conta">Objeto Conta com os dados atualizados.</param>
        public void Update(Conta conta) { _context.Contas.Update(conta); _context.SaveChanges(); }

        /// <summary>
        /// Verifica se uma conta existe no banco de dados pelo número da conta.
        /// </summary>
        /// <param name="numeroConta">Número da conta a ser verificada.</param>
        /// <returns>True se a conta existir, caso contrário, false.</returns>
        public bool Exists(int numeroConta) => _context.Contas.Any(c => c.NumeroConta == numeroConta);
    }
}