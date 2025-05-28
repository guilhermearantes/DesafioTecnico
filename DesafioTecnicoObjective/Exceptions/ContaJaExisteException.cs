namespace DesafioTecnicoObjective.Exceptions
{
    /// <summary>
    /// Exceção lançada quando uma tentativa de criar uma conta que já existe é feita.
    /// </summary>
    public class ContaJaExisteException : Exception
    {
        public ContaJaExisteException() : base("Conta já existe.") { }

        public ContaJaExisteException(string message) : base(message) { }

        public ContaJaExisteException(string message, Exception innerException) : base(message, innerException) { }
    }
}
