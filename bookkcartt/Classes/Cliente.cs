using bookkcartt.Classes;
using System;

namespace bookkcartt.Classes
{
    public class Cliente : Pessoa
    {
        public bool VIP { get; set; }

        public Cliente()
        {
        }

        public Cliente(int id, string nome, string email, bool vip)
            : base(id, nome, email)
        {
            VIP = vip;
        }

        public virtual decimal PercentualDesconto()
        {
            return 0m;
        }

        public override string Descricao()
        {
            return "Cliente: " + Nome +
                   " | E-mail: " + Email +
                   " | VIP: " + (VIP ? "Sim" : "Não");
        }
    }

    public class ClienteVip : Cliente
    {
        public ClienteVip()
        {
        }

        public ClienteVip(int id, string nome, string email)
            : base(id, nome, email, true)
        {
        }

        public override decimal PercentualDesconto()
        {
            return 0.10m;
        }

        public override string Descricao()
        {
            return "Cliente VIP: " + Nome +
                   " | E-mail: " + Email +
                   " | Desconto: 10%";
        }
    }
}