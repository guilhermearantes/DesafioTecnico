using System.Text.Json.Serialization;

namespace DesafioTecnicoObjective.DTOs
{
    /// <summary>
    /// DTO utilizado para retornar informações de uma conta em respostas de serviços ou APIs.
    /// </summary>
    public class ContaResponseDto
    {
        /// <summary>
        /// Número identificador da conta.
        /// </summary>
        [JsonPropertyName("numero_conta")]
        public int NumeroConta { get; set; }

        /// <summary>
        /// Saldo atual da conta.
        /// </summary>
        [JsonPropertyName("saldo")]
        public float Saldo { get; set; }
    }
}
