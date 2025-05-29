namespace DesafioTecnicoObjective.Services.Strategies
{
    /// <summary>
    /// Decorador de estratégia de taxa que adiciona logs ao processo de cálculo de taxa.
    /// Escreve no console antes e depois do cálculo da taxa.
    /// </summary>
    public class LogTaxaStrategyDecorator : TaxaStrategyDecorator
    {
        public LogTaxaStrategyDecorator(ITaxaStrategy inner) : base(inner) { }

        public override float CalcularTaxa(float valor)
        {
            Console.WriteLine($"[LOG] Calculando taxa para valor: {valor}");
            var taxa = base.CalcularTaxa(valor);
            Console.WriteLine($"[LOG] Taxa calculada: {taxa}");
            return taxa;
        }
    }
}
