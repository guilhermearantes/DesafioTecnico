namespace DesafioTecnicoObjective.Services.Strategies
{
    public class CreditoStrategy : ITaxaStrategy
    {
        public float CalcularTaxa(float valor) => valor * 0.05f;
    }
}
