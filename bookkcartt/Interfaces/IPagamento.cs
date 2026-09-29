
namespace bookkcartt.Interfaces
{
    public interface IPagamento
    {
        string Nome { get; }
        decimal CalcularValorFinal(decimal valor);
        void RealizarPagamento(decimal valor);
    }
}