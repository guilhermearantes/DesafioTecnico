using System.Net;

namespace DesafioTecnicoObjective.Exceptions
{
    /// <summary>
    /// Exceção lançada quando não há saldo suficiente para a transação.
    /// </summary>
    public class SaldoInsuficienteException : Exception
    {
        public int StatusCode { get; } = 404;

        public SaldoInsuficienteException() : base("Saldo insuficiente.") { }
        public SaldoInsuficienteException(string message) : base(message) { }
        public SaldoInsuficienteException(string message, Exception innerException) : base(message, innerException) { }
    }
}

