namespace DesafioTecnicoObjective.Services
{
    public class CreditoStrategy : ITaxaStrategy
    {
        public float CalcularTaxa(float valor) => valor * 0.05f;
    }
}
