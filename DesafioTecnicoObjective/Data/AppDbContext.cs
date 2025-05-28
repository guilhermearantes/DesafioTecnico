using Microsoft.EntityFrameworkCore;
using DesafioTecnicoObjective.Models;

namespace DesafioTecnicoObjective.Data
{
    /// <summary>
    /// Contexto do Entity Framework responsável pelo acesso ao banco de dados da aplicação.
    /// </summary>
    public class AppDbContext : DbContext
    {
        /// <summary>
        /// Construtor que recebe as opções de configuração do contexto.
        /// </summary>
        /// <param name="options">Opções de configuração do DbContext.</param>
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        /// <summary>
        /// Conjunto de contas no banco de dados.
        /// </summary>
        public DbSet<Conta> Contas { get; set; }

    }
}
