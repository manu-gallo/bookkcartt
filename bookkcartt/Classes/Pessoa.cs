using bookkcartt.Services;
using System;

namespace bookkcartt.Classes
{
    public class Pessoa
    {
        private string nome;
        private string email;

        public int Id { get; private set; }

        public string Nome
        {
            get { return nome; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Nome é obrigatório.");

                nome = value.Trim();
            }
        }

        public string Email
        {
            get { return email; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("E-mail inválido.");

                try
                {
                    var _ = new System.Net.Mail.MailAddress(value);
                    email = value.Trim();
                }
                catch
                {
                    throw new ArgumentException("E-mail inválido.");
                }
            }
        }

        public Pessoa()
        {
        }

        public Pessoa(int id, string nome, string email)
        {
            Id = id;
            Nome = nome;
            Email = email;
        }

        public virtual string Descricao()
        {
            return Nome + " - " + Email;
        }
    }
}