using System;

namespace bookkcartt.Classes
{
    public class EstoqueInsuficienteException : Exception
    {
        public EstoqueInsuficienteException(string mensagem)
            : base(mensagem)
        {
        }
    }
}