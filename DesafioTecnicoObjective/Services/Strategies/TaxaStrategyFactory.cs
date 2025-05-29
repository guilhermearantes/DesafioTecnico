using DesafioTecnicoObjective.Exceptions;

namespace DesafioTecnicoObjective.Services.Strategies
{
    public static class TaxaStrategyFactory
    {
        /// <summary>
        /// Retorna a estratégia de taxa conforme a forma de pagamento.
        /// Lança InvalidOperationException se a forma de pagamento for inválida.
        /// </summary>
        public static ITaxaStrategy GetStrategy(string formaPagamento)
        {
            return formaPagamento.ToUpper() switch
            {
                "D" => new DebitoStrategy(),
                "C" => new CreditoStrategy(),
                "P" => new PixStrategy(),
                _ => throw new FormaPagamentoInvalidoException("Forma de pagamento inválida. Informe D (debito), C (credito) ou P (pix).")
            };
        }
    }
}
