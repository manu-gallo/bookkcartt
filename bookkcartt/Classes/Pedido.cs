using bookkcartt.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace bookkcartt.Classes
{
    public enum StatusPedido
    {
        Criado = 1,
        Pago = 2,
        Cancelado = 3
    }

    public class Pedido
    {
        public int Id { get; set; }

        public int ClienteId { get; set; }

        public List<ItemPedido> Itens { get; set; }

        public StatusPedido Status { get; set; }

        public string FormaPagamento { get; set; }

        public decimal Desconto { get; set; }

        public decimal ValorFinal { get; set; }

        public decimal Subtotal
        {
            get
            {
                decimal total = 0;

                foreach (ItemPedido item in Itens)
                {
                    total += item.Subtotal;
                }

                return total;
            }
        }

        public Pedido()
        {
            Itens = new List<ItemPedido>();
            Status = StatusPedido.Criado;
        }

        public void AdicionarItem(ItemPedido item)
        {
            if (item == null)
                throw new ArgumentNullException("item");

            Itens.Add(item);
        }

        public override string ToString()
        {
            StringBuilder texto = new StringBuilder();

            texto.AppendLine("===== PEDIDO #" + Id + " =====");
            texto.AppendLine("Cliente: #" + ClienteId);
            texto.AppendLine("Status: " + Status);
            texto.AppendLine("Pagamento: " + FormaPagamento);
            texto.AppendLine();

            foreach (ItemPedido item in Itens)
            {
                texto.AppendLine(
                    item.Titulo +
                    " | " +
                    item.Quantidade +
                    " x " +
                    Validador.Moeda(item.PrecoUnitario) +
                    " = " +
                    Validador.Moeda(item.Subtotal)
                );
            }

            texto.AppendLine();
            texto.AppendLine("Subtotal: " + Validador.Moeda(Subtotal));
            texto.AppendLine("Desconto: " + Validador.Moeda(Desconto));
            texto.AppendLine("Valor final: " + Validador.Moeda(ValorFinal));

            return texto.ToString();
        }
    }
}