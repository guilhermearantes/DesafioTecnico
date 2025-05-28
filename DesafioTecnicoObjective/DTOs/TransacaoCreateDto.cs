using System.Text.Json.Serialization;

namespace DesafioTecnicoObjective.DTOs
{
    /// <summary>
    /// DTO utilizado para criar uma transação em uma conta bancária, contendo os dados necessários para a operação.
    /// </summary>
    public class TransacaoCreateDto
    {
        /// <summary>
        /// Construtor sem parâmetros necessário para o binding automático de frameworks como ASP.NET.
        /// Inicializa a propriedade FormaPagamento como string vazia.
        /// </summary>
        public TransacaoCreateDto()
        {
            FormaPagamento = string.Empty; 
        }

        /// <summary>
        /// Construtor que inicializa todas as propriedades do DTO de transação.
        /// </summary>
        /// <param name="formaPagamento">Forma de pagamento da transação (ex: D, C, P).</param>
        /// <param name="numeroConta">Número da conta a ser movimentada.</param>
        /// <param name="valor">Valor da transação.</param>
        public TransacaoCreateDto(string formaPagamento, int numeroConta, float valor)
        {
            FormaPagamento = formaPagamento;
            NumeroConta = numeroConta;
            Valor = valor;
        }

        /// <summary>
        /// Forma de pagamento. Sendo P => Pix, C => Cartão de Crédito e D => Cartão de Débito
        /// </summary>
        [JsonPropertyName("forma_pagamento")]
        public string FormaPagamento { get; set; }

        /// <summary>
        /// Número identificador da conta a ser criada.
        /// </summary>
        [JsonPropertyName("numero_conta")]
        public int NumeroConta { get; set; }

        /// <summary>
        /// Valor da transação.
        /// </summary>
        [JsonPropertyName("valor")]
        public float Valor { get; set; }
    }
}   