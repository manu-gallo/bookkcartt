
using bookkcartt.Classes;
using bookkcartt.Interfaces;
using bookkcartt.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace bookkcartt.Services
{
    public class Livraria
    {
        public List<Livro> Livros { get; set; }

        public List<Cliente> Clientes { get; set; }

        public List<Pedido> Pedidos { get; set; }

        private static int proximoLivroId = 1;
        private static int proximoClienteId = 1;
        private static int proximoPedidoId = 1;

        public Livraria()
        {
            Livros = new List<Livro>();
            Clientes = new List<Cliente>();
            Pedidos = new List<Pedido>();
        }

        public void CarregarCatalogoInicial()
        {
            if (Livros.Count > 0)
                return;

            CadastrarLivro(
                "É Assim que Acaba",
                "Colleen Hoover",
                Genero.RomanceDrama,
                49.90m,
                10);

            CadastrarLivro(
                "Verity",
                "Colleen Hoover",
                Genero.SuspenseThriller,
                44.90m,
                8);

            CadastrarLivro(
                "Harry Potter e a Pedra Filosofal",
                "J.K. Rowling",
                Genero.Fantasia,
                39.90m,
                12);

            CadastrarLivro(
                "Corte de Espinhos e Rosas",
                "Sarah J. Maas",
                Genero.FantasiaRomance,
                59.90m,
                7);

            CadastrarLivro(
                "Quarta Asa",
                "Rebecca Yarros",
                Genero.FantasiaRomance,
                69.90m,
                5);

            CadastrarLivro(
                "Uma Família Feliz",
                "Raphael Montes",
                Genero.SuspenseThriller,
                42.90m,
                6);
        }

        public Cliente CadastrarCliente(
            string nome,
            string email,
            bool vip)
        {
            int id = proximoClienteId++;

            Cliente cliente;

            if (vip)
            {
                cliente = new ClienteVip(
                    id,
                    nome,
                    email);
            }
            else
            {
                cliente = new Cliente(
                    id,
                    nome,
                    email,
                    false);
            }

            Clientes.Add(cliente);

            return cliente;
        }

        public Livro CadastrarLivro(
            string titulo,
            string autor,
            Genero genero,
            decimal preco,
            int estoque)
        {
            Livro livro = new Livro(
                proximoLivroId++,
                titulo,
                autor,
                genero,
                preco,
                estoque);

            Livros.Add(livro);

            return livro;
        }

        public List<Livro> BuscarLivros(string termo)
        {
            if (string.IsNullOrWhiteSpace(termo))
                return new List<Livro>();

            termo = termo.Trim().ToLower();

            return Livros
                .Where(x =>
                    x.Titulo.ToLower().Contains(termo) ||
                    x.Autor.ToLower().Contains(termo) ||
                    Validador.NomeGenero(x.Genero)
                        .ToLower()
                        .Contains(termo))
                .ToList();
        }

        public Livro BuscarLivro(int id)
        {
            return Livros.FirstOrDefault(x => x.Id == id);
        }

        public Cliente BuscarCliente(int id)
        {
            return Clientes.FirstOrDefault(x => x.Id == id);
        }

        public Pedido BuscarPedido(int id)
        {
            return Pedidos.FirstOrDefault(x => x.Id == id);
        }

        public void RemoverCliente(int id)
        {
            Cliente cliente = BuscarCliente(id);

            if (cliente == null)
                throw new KeyNotFoundException(
                    "Cliente não encontrado.");

            Clientes.Remove(cliente);
        }

        public void RemoverLivro(int id)
        {
            Livro livro = BuscarLivro(id);

            if (livro == null)
                throw new KeyNotFoundException(
                    "Livro não encontrado.");

            Livros.Remove(livro);
        }

        public Pedido Comprar(
            int clienteId,
            List<Tuple<int, int>> itens,
            IPagamento pagamento)
        {
            Cliente cliente = BuscarCliente(clienteId);

            if (cliente == null)
                throw new KeyNotFoundException(
                    "Cliente não encontrado.");

            if (itens == null || itens.Count == 0)
                throw new ArgumentException(
                    "Adicione pelo menos um livro ao pedido.");

            if (pagamento == null)
                throw new ArgumentNullException(
                    "pagamento");

            Pedido pedido = new Pedido();

            pedido.Id = proximoPedidoId++;
            pedido.ClienteId = clienteId;
            pedido.FormaPagamento = pagamento.Nome;

            decimal subtotal = 0;

            foreach (Tuple<int, int> item in itens)
            {
                Livro livro = BuscarLivro(item.Item1);

                if (livro == null)
                    throw new KeyNotFoundException(
                        "Livro não encontrado: " + item.Item1);

                int quantidade = item.Item2;

                if (quantidade <= 0)
                    throw new ArgumentException(
                        "A quantidade deve ser maior que zero.");

                if (quantidade > livro.Estoque)
                    throw new EstoqueInsuficienteException(
                        "Estoque insuficiente para: " +
                        livro.Titulo);

                ItemPedido itemPedido =
                    new ItemPedido(
                        livro.Id,
                        livro.Titulo,
                        livro.Preco,
                        quantidade);

                pedido.AdicionarItem(itemPedido);

                subtotal += itemPedido.Subtotal;
            }

            decimal descontoCliente =
                subtotal * cliente.PercentualDesconto();

            decimal valorComDesconto =
                subtotal - descontoCliente;

            decimal valorFinal =
                pagamento.CalcularValorFinal(valorComDesconto);

            pedido.Desconto =
                subtotal - valorFinal;

            pedido.ValorFinal = valorFinal;

            foreach (Tuple<int, int> item in itens)
            {
                Livro livro = BuscarLivro(item.Item1);
                livro.BaixarEstoque(item.Item2);
            }

            pagamento.RealizarPagamento(valorFinal);

            pedido.Status = StatusPedido.Pago;

            Pedidos.Add(pedido);

            return pedido;
        }

        public void CancelarPedido(int id)
        {
            Pedido pedido = BuscarPedido(id);

            if (pedido == null)
                throw new KeyNotFoundException(
                    "Pedido não encontrado.");

            if (pedido.Status == StatusPedido.Cancelado)
                throw new InvalidOperationException(
                    "Este pedido já foi cancelado.");

            foreach (ItemPedido item in pedido.Itens)
            {
                Livro livro = BuscarLivro(item.LivroId);

                if (livro != null)
                    livro.Repor(item.Quantidade);
            }

            pedido.Status = StatusPedido.Cancelado;
        }

        public List<Livro> LivrosComEstoqueBaixo()
        {
            return Livros
                .Where(x => x.EstoqueBaixo())
                .ToList();
        }
    }
}