namespace DesafioTecnicoObjective.Services.Strategies
{
    public class TaxaStrategyDecorator : ITaxaStrategy
    {
        protected readonly ITaxaStrategy _inner;

        public TaxaStrategyDecorator(ITaxaStrategy inner)
        {
            _inner = inner;
        }

        public virtual float CalcularTaxa(float valor)
        {
            return _inner.CalcularTaxa(valor);
        }
    }
}
