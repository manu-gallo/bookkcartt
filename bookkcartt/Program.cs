
using bookkcartt.Classes;
using bookkcartt.Interfaces;
using bookkcartt.Services;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace bookkcartt
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding =
                System.Text.Encoding.UTF8;

            Livraria livraria;

            if (Armazenamento.Existe())
            {
                try
                {
                    livraria =
                        Armazenamento.Carregar();

                    Console.WriteLine(
                        "Dados carregados de dados.json.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(
                        "Erro ao carregar: " +
                        ex.Message);

                    livraria = new Livraria();
                    livraria.CarregarCatalogoInicial();
                }
            }
            else
            {
                livraria = new Livraria();
                livraria.CarregarCatalogoInicial();
            }

            int opcao = -1;

            while (opcao != 0)
            {
                Console.WriteLine();
                Console.WriteLine(
                    "====================================");
                Console.WriteLine(
                    "      BOOKCARTT - LIVRARIA VIRTUAL");
                Console.WriteLine(
                    "====================================");
                Console.WriteLine(
                    "1  - Cadastrar cliente");
                Console.WriteLine(
                    "2  - Cadastrar livro");
                Console.WriteLine(
                    "3  - Listar catálogo");
                Console.WriteLine(
                    "4  - Buscar livro");
                Console.WriteLine(
                    "5  - Listar clientes");
                Console.WriteLine(
                    "6  - Comprar livros");
                Console.WriteLine(
                    "7  - Listar pedidos");
                Console.WriteLine(
                    "8  - Cancelar pedido");
                Console.WriteLine(
                    "9  - Excluir cadastro");
                Console.WriteLine(
                    "10 - Alerta de estoque mínimo");
                Console.WriteLine(
                    "11 - Salvar dados");
                Console.WriteLine(
                    "12 - Carregar dados");
                Console.WriteLine(
                    "0  - Sair");

                opcao =
                    LerInt("\nDigite uma opção: ");

                try
                {
                    switch (opcao)
                    {
                        case 1:
                            CadastrarCliente(livraria);
                            break;

                        case 2:
                            CadastrarLivro(livraria);
                            break;

                        case 3:
                            ListarLivros(livraria);
                            break;

                        case 4:
                            BuscarLivro(livraria);
                            break;

                        case 5:
                            ListarClientes(livraria);
                            break;

                        case 6:
                            Comprar(livraria);
                            break;

                        case 7:
                            ListarPedidos(livraria);
                            break;

                        case 8:
                            CancelarPedido(livraria);
                            break;

                        case 9:
                            ExcluirCadastro(livraria);
                            break;

                        case 10:
                            EstoqueBaixo(livraria);
                            break;

                        case 11:
                            Armazenamento.Salvar(
                                livraria);

                            Console.WriteLine(
                                "Dados salvos em dados.json.");

                            break;

                        case 12:
                            livraria =
                                Armazenamento.Carregar();

                            Console.WriteLine(
                                "Dados carregados.");

                            break;

                        case 0:
                            Console.WriteLine(
                                "Até logo!");

                            break;

                        default:
                            Console.WriteLine(
                                "Opção inválida.");

                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.ForegroundColor =
                        ConsoleColor.Red;

                    Console.WriteLine(
                        "Erro: " +
                        ex.Message);

                    Console.ResetColor();
                }
            }
        }

        static void CadastrarCliente(
            Livraria livraria)
        {
            string nome =
                Ler("Nome: ");

            string email =
                Ler("E-mail: ");

            string resposta =
                Ler("Cliente VIP (10% de desconto)? (s/n): ");

            bool vip =
                resposta.ToLower() == "s";

            Cliente cliente =
                livraria.CadastrarCliente(
                    nome,
                    email,
                    vip);

            Console.WriteLine(
                "Cliente cadastrado: " +
                cliente.Descricao() +
                " | ID: " +
                cliente.Id);
        }

        static void CadastrarLivro(
            Livraria livraria)
        {
            string titulo =
                Ler("Título: ");

            string autor =
                Ler("Autor: ");

            Console.WriteLine();
            Console.WriteLine("Gêneros:");

            foreach (Genero genero in
                Enum.GetValues(typeof(Genero)))
            {
                Console.WriteLine(
                    (int)genero +
                    " - " +
                    Validador.NomeGenero(genero));
            }

            int generoNumero =
                LerInt("Gênero (número): ");

            if (!Enum.IsDefined(
                typeof(Genero),
                generoNumero))
            {
                throw new ArgumentException(
                    "Gênero inválido.");
            }

            Genero generoSelecionado =
                (Genero)generoNumero;

            decimal preco =
                LerDecimal("Preço (R$): ");

            int estoque =
                LerInt("Estoque inicial: ");

            Livro livro =
                livraria.CadastrarLivro(
                    titulo,
                    autor,
                    generoSelecionado,
                    preco,
                    estoque);

            Console.WriteLine(
                "Livro cadastrado com ID: " +
                livro.Id);
        }

        static void ListarLivros(
            Livraria livraria)
        {
            if (livraria.Livros.Count == 0)
            {
                Console.WriteLine(
                    "Nenhum livro cadastrado.");

                return;
            }

            Console.WriteLine();
            Console.WriteLine("===== CATÁLOGO =====");

            foreach (Livro livro in
                livraria.Livros)
            {
                Console.WriteLine(livro);
            }
        }

        static void BuscarLivro(
            Livraria livraria)
        {
            string termo =
                Ler("Digite título, autor ou gênero: ");

            List<Livro> encontrados =
                livraria.BuscarLivros(termo);

            if (encontrados.Count == 0)
            {
                Console.WriteLine(
                    "Nenhum livro encontrado.");

                return;
            }

            foreach (Livro livro in
                encontrados)
            {
                Console.WriteLine(livro);
            }
        }

        static void ListarClientes(
            Livraria livraria)
        {
            if (livraria.Clientes.Count == 0)
            {
                Console.WriteLine(
                    "Nenhum cliente cadastrado.");

                return;
            }

            Console.WriteLine();
            Console.WriteLine("===== CLIENTES =====");

            foreach (Cliente cliente in
                livraria.Clientes)
            {
                Console.WriteLine(
                    "#" +
                    cliente.Id +
                    " | " +
                    cliente.Descricao());
            }
        }

        static void Comprar(
            Livraria livraria)
        {
            if (livraria.Clientes.Count == 0)
            {
                throw new InvalidOperationException(
                    "Cadastre um cliente antes de comprar.");
            }

            int clienteId =
                LerInt("ID do cliente: ");

            List<Tuple<int, int>> itens =
                new List<Tuple<int, int>>();

            while (true)
            {
                int livroId =
                    LerInt(
                        "ID do livro (0 para finalizar): ");

                if (livroId == 0)
                    break;

                int quantidade =
                    LerInt("Quantidade: ");

                itens.Add(
                    new Tuple<int, int>(
                        livroId,
                        quantidade));
            }

            if (itens.Count == 0)
            {
                throw new ArgumentException(
                    "Nenhum livro foi adicionado.");
            }

            Console.WriteLine();
            Console.WriteLine(
                "Pagamento:");
            Console.WriteLine(
                "1 - Pix (5% de desconto)");
            Console.WriteLine(
                "2 - Cartão");
            Console.WriteLine(
                "3 - Boleto");

            int forma =
                LerInt("Forma de pagamento: ");

            IPagamento pagamento;

            switch (forma)
            {
                case 1:
                    pagamento =
                        new PagamentoPix();
                    break;

                case 2:
                    int parcelas =
                        LerInt("Parcelas (1-6): ");

                    pagamento =
                        new PagamentoCartao(
                            parcelas);

                    break;

                case 3:
                    pagamento =
                        new PagamentoBoleto();

                    break;

                default:
                    throw new ArgumentException(
                        "Forma de pagamento inválida.");
            }

            Pedido pedido =
                livraria.Comprar(
                    clienteId,
                    itens,
                    pagamento);

            Console.WriteLine();
            Console.WriteLine(pedido);
        }

        static void ListarPedidos(
            Livraria livraria)
        {
            if (livraria.Pedidos.Count == 0)
            {
                Console.WriteLine(
                    "Nenhum pedido realizado.");

                return;
            }

            foreach (Pedido pedido in
                livraria.Pedidos)
            {
                Console.WriteLine(pedido);
                Console.WriteLine();
            }
        }

        static void CancelarPedido(
            Livraria livraria)
        {
            int id =
                LerInt("ID do pedido: ");

            livraria.CancelarPedido(id);

            Console.WriteLine(
                "Pedido cancelado e estoque devolvido.");
        }

        static void ExcluirCadastro(
            Livraria livraria)
        {
            string tipo =
                Ler(
                    "Excluir (c)liente ou (l)ivro? ");

            if (tipo.ToLower() == "c")
            {
                int id =
                    LerInt("ID do cliente: ");

                livraria.RemoverCliente(id);

                Console.WriteLine(
                    "Cliente removido.");
            }
            else if (tipo.ToLower() == "l")
            {
                int id =
                    LerInt("ID do livro: ");

                livraria.RemoverLivro(id);

                Console.WriteLine(
                    "Livro removido.");
            }
            else
            {
                throw new ArgumentException(
                    "Opção inválida.");
            }
        }

        static void EstoqueBaixo(
            Livraria livraria)
        {
            List<Livro> baixos =
                livraria.LivrosComEstoqueBaixo();

            if (baixos.Count == 0)
            {
                Console.WriteLine(
                    "Todos os livros estão com estoque saudável.");

                return;
            }

            Console.WriteLine();
            Console.WriteLine(
                "ATENÇÃO - LIVROS PARA REPOR:");

            foreach (Livro livro in baixos)
            {
                Console.WriteLine(
                    livro.Titulo +
                    " - estoque: " +
                    livro.Estoque +
                    " | mínimo: " +
                    livro.EstoqueMinimo);
            }

            string resposta =
                Ler("Repor algum? (s/n): ");

            if (resposta.ToLower() == "s")
            {
                int id =
                    LerInt("ID do livro: ");

                Livro livro =
                    livraria.BuscarLivro(id);

                if (livro == null)
                {
                    throw new KeyNotFoundException(
                        "Livro não encontrado.");
                }

                int quantidade =
                    LerInt(
                        "Quantidade a repor: ");

                livro.Repor(quantidade);

                Console.WriteLine(
                    "Estoque atualizado.");
            }
        }

        static string Ler(string mensagem)
        {
            Console.Write(mensagem);

            string valor =
                Console.ReadLine();

            if (valor == null)
                return "";

            return valor;
        }

        static int LerInt(string mensagem)
        {
            Console.Write(mensagem);

            int valor;

            if (int.TryParse(
                Console.ReadLine(),
                out valor))
            {
                return valor;
            }

            throw new FormatException(
                "Digite um número inteiro válido.");
        }

        static decimal LerDecimal(
            string mensagem)
        {
            Console.Write(mensagem);

            string texto =
                Console.ReadLine();

            if (texto == null)
                throw new FormatException(
                    "Digite um valor válido.");

            texto =
                texto.Replace(',', '.');

            decimal valor;

            if (decimal.TryParse(
                texto,
                NumberStyles.Number,
                CultureInfo.InvariantCulture,
                out valor))
            {
                return valor;
            }

            throw new FormatException(
                "Digite um valor numérico válido.");
        }
    }
}