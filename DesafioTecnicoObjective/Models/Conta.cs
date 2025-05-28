using System.ComponentModel.DataAnnotations;

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
        public int NumeroConta { get; set; }

        /// <summary>
        /// Saldo atual da conta.
        /// </summary>
        public float Saldo { get; set; }
    }
}
