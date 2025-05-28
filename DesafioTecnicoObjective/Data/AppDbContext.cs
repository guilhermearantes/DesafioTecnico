using Microsoft.EntityFrameworkCore;
using DesafioTecnicoObjective.Models;

namespace DesafioTecnicoObjective.Data
{
    /// <summary>
    /// Contexto do Entity Framework respons�vel pelo acesso ao banco de dados da aplica��o.
    /// </summary>
    public class AppDbContext : DbContext
    {
        /// <summary>
        /// Construtor que recebe as op��es de configura��o do contexto.
        /// </summary>
        /// <param name="options">Op��es de configura��o do DbContext.</param>
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        /// <summary>
        /// Conjunto de contas no banco de dados.
        /// </summary>
        public DbSet<Conta> Contas { get; set; }

    }
}
