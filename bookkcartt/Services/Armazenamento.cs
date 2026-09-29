using bookkcartt.Classes;
using bookkcartt.Services;
using System;
using System.IO;
using Newtonsoft.Json;

namespace bookkcartt.Services
{
    public static class Armazenamento
    {
        private static readonly string Caminho =
            "dados.json";

        public static bool Existe()
        {
            return File.Exists(Caminho);
        }

        public static void Salvar(Livraria livraria)
        {
            string json = JsonConvert.SerializeObject(
                livraria,
                Formatting.Indented);

            File.WriteAllText(Caminho, json);
        }

        public static Livraria Carregar()
        {
            if (!File.Exists(Caminho))
                throw new FileNotFoundException(
                    "Arquivo dados.json não encontrado.");

            string json =
                File.ReadAllText(Caminho);

            Livraria livraria =
                JsonConvert.DeserializeObject<Livraria>(
                    json);

            if (livraria == null)
                throw new InvalidOperationException(
                    "Não foi possível carregar os dados.");

            return livraria;
        }
    }
}