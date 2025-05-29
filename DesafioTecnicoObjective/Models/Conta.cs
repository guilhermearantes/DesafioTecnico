using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DesafioTecnicoObjective.Models
{
    /// <summary>
    /// Entidade que representa uma conta bancária, contendo número identificador e saldo atual.
    /// </summary>
    public class Conta
    {
        /// <summary>
        /// Número identificador da conta.
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int NumeroConta { get; set; }

        /// <summary>
        /// Saldo atual da conta.
        /// </summary>
        public float Saldo { get; set; }
    }
}
