namespace DesafioTecnicoObjective.Services
{
    public class DebitoStrategy : ITaxaStrategy
    {
        public float CalcularTaxa(float valor) => valor * 0.03f;
    }
}
