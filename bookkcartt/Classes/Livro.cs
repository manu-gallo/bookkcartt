using bookkcartt.Classes;
using bookkcartt.Services;
using System;

namespace bookkcartt.Classes
{
    public class Livro
    {
        public int Id { get; private set; }

        public string Titulo { get; set; }

        public string Autor { get; set; }

        public Genero Genero { get; set; }

        public decimal Preco { get; set; }

        public int Estoque { get; private set; }

        public int EstoqueMinimo { get; set; }

        public Livro()
        {
        }

        public Livro(
            int id,
            string titulo,
            string autor,
            Genero genero,
            decimal preco,
            int estoque)
        {
            if (string.IsNullOrWhiteSpace(titulo))
                throw new ArgumentException("Título é obrigatório.");

            if (string.IsNullOrWhiteSpace(autor))
                throw new ArgumentException("Autor é obrigatório.");

            if (preco <= 0)
                throw new ArgumentException("Preço deve ser maior que zero.");

            if (estoque < 0)
                throw new ArgumentException("Estoque não pode ser negativo.");

            Id = id;
            Titulo = titulo;
            Autor = autor;
            Genero = genero;
            Preco = preco;
            Estoque = estoque;
            EstoqueMinimo = 2;
        }

        public bool EstoqueBaixo()
        {
            return Estoque <= EstoqueMinimo;
        }

        public void BaixarEstoque(int quantidade)
        {
            if (quantidade <= 0)
                throw new ArgumentException("Quantidade deve ser maior que zero.");

            if (quantidade > Estoque)
                throw new EstoqueInsuficienteException(
                    "Estoque insuficiente para o livro: " + Titulo);

            Estoque -= quantidade;
        }

        public void Repor(int quantidade)
        {
            if (quantidade <= 0)
                throw new ArgumentException("Quantidade deve ser maior que zero.");

            Estoque += quantidade;
        }

        public override string ToString()
        {
            return "#" + Id +
                   " | " + Titulo +
                   " | " + Autor +
                   " | " + Validador.NomeGenero (Genero) +
                   " | " + Validador.Moeda(Preco) +
                   " | Estoque: " + Estoque;
        }
    }
}
