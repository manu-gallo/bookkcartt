namespace bookkcartt.Classes
{
    public class ItemPedido
    {
        public int LivroId { get; set; }

        public string Titulo { get; set; }

        public decimal PrecoUnitario { get; set; }

        public int Quantidade { get; set; }

        public decimal Subtotal
        {
            get { return PrecoUnitario * Quantidade; }
        }

        public ItemPedido()
        {
        }

        public ItemPedido(
            int livroId,
            string titulo,
            decimal precoUnitario,
            int quantidade)
        {
            LivroId = livroId;
            Titulo = titulo;
            PrecoUnitario = precoUnitario;
            Quantidade = quantidade;
        }
    }
}