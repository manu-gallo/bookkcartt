using System;
using System.Globalization;
using System.Text.RegularExpressions;
using bookkcartt.Classes;

namespace bookkcartt.Services
{
    public static class Validador
    {
        private static readonly CultureInfo Br =
            new CultureInfo("pt-BR");

        public static string TextoObrigatorio(
            string valor,
            string campo)
        {
            if (string.IsNullOrWhiteSpace(valor))
            {
                throw new ArgumentException(
                    campo + " é obrigatório.");
            }

            return valor.Trim();
        }

        public static bool EmailValido(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return false;
            }

            return Regex.IsMatch(
                email.Trim(),
                @"^[^@\s]+@[^@\s]+\.[^@\s]+$");
        }

        public static string Moeda(decimal valor)
        {
            return valor.ToString("C2", Br);
        }

        public static string NomeGenero(Genero genero)
        {
            switch (genero)
            {
                case Genero.Romance:
                    return "Romance";

                case Genero.RomanceDrama:
                    return "Romance/drama";

                case Genero.SuspenseThriller:
                    return "Suspense/thriller";

                case Genero.Fantasia:
                    return "Fantasia";

                case Genero.FantasiaDarkRomance:
                    return "Fantasia/dark romance";

                case Genero.FantasiaRomance:
                    return "Fantasia/romance";

                default:
                    return genero.ToString();
            }
        }
    }
}