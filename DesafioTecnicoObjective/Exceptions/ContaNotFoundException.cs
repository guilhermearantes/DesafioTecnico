using System;

namespace DesafioTecnicoObjective.Exceptions
{
    /// <summary>
    /// Exceção lançada quando uma conta bancária não é encontrada.
    /// </summary>
    public class ContaNotFoundException : Exception
    {
        public ContaNotFoundException() : base("Conta não encontrada.") { }

        public ContaNotFoundException(string message)
            : base(message)
        {
        }

        public ContaNotFoundException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
