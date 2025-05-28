using System.Text.Json.Serialization;

namespace DesafioTecnicoObjective.DTOs
{
    /// <summary>
    /// DTO utilizado para criar uma nova conta, contendo os dados necessários para a operação.
    /// </summary>
    public class ContaCreateDto
    {
        /// <summary>
        /// Número identificador da conta a ser criada.
        /// </summary>
        [JsonPropertyName("numero_conta")]
        public int NumeroConta { get; set; }

        /// <summary>
        /// Saldo inicial da conta.
        /// </summary>
        [JsonPropertyName("saldo")]
        public float Saldo { get; set; }
    }
}