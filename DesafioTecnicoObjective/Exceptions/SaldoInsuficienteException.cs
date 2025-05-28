namespace DesafioTecnicoObjective.Exceptions
{
    /// <summary>
    /// Exceção lançada quando uma conta não possui saldo suficiente para realizar uma transação.
    /// </summary>
    public class SaldoInsuficienteException : Exception
    {
        public SaldoInsuficienteException() { }

        public SaldoInsuficienteException(string message) : base(message) { }

        public SaldoInsuficienteException(string message, Exception inner) : base(message, inner) { }
    }
}

