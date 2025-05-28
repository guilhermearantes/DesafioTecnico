using Microsoft.EntityFrameworkCore;
using DesafioTecnicoObjective.Models;
using DesafioTecnicoObjective.Repositories;

/// <summary>
/// Contexto do Entity Framework para acesso ao banco de dados de contas.
/// </summary>
public class ContaDbContext : DbContext
{
    /// <summary>
    /// Construtor que recebe as opções de configuração do contexto.
    /// </summary>
    /// <param name="options">Opções de configuração do DbContext.</param>
    public ContaDbContext(DbContextOptions<ContaDbContext> options) : base(options) { }
    
    /// <summary>
    /// Conjunto de contas no banco de dados.
    /// </summary>
    public DbSet<Conta> Contas { get; set; }
}
