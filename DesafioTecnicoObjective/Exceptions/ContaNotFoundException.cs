using System;

namespace DesafioTecnicoObjective.Exceptions
{
    /// <summary>
    /// Exce��o lan�ada quando uma conta banc�ria n�o � encontrada.
    /// </summary>
    public class ContaNotFoundException : Exception
    {
        public ContaNotFoundException() : base("Conta n�o encontrada.") { }

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
