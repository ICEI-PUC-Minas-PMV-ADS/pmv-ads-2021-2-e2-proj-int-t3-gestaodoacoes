using System;
using System.Linq;
using doee.Models;

namespace doee.Data
{
    public class DbInitializer
    {
        public static void Initialize(DoeeContext context)
        {
            context.Database.EnsureCreated();

            // Look for any students.
            if (context.Instituicoes.Any())
            {
                return;   // DB has been seeded
            }

            //Instituicao
            var instituicoes = new Instituicao[]
            {
            new Instituicao{Nome ="Carson",CNPJ="111",setor = Setor.Alimentacao, Descricao= "Espaço de refeição para aqueles que tem muito apetite e pouca oportunidade", MetaArrecadacao = 10, DataRegistro =  DateTime.Parse("2015-09-01"), Email = "111@gmail.com", senha="111"},
            new Instituicao{Nome ="X-sport",CNPJ="222",setor = Setor.Esporte, Descricao= "Desenvolve atividades e um ambiente de competição com crianças do morro do RJ", MetaArrecadacao = 20,DataRegistro =  DateTime.Parse("2020-09-01"), Email = "222@gmail.com", senha="222"}
            };

            foreach (Instituicao i in instituicoes)
            {
                context.Instituicoes.Add(i);
            }
            context.SaveChanges();

            //doação
            var doacoes = new Doacao[]
           {
            new Doacao{Data = DateTime.Parse("2021-09-01"), NomeDoador="Igor Vicente",Email="iv@gmail.com",Cpf="11122233344",NomeCartao="IGORO V", NumeroCartao = "234252352134", Validade = "10/10", CodSeguranca = 111, Valor = 100, BeneficiarioCNPJ="111"},
            new Doacao{Data = DateTime.Parse("2021-09-01"), NomeDoador="Igor Machado",Email="im@gmail.com",Cpf="22233344455",NomeCartao="IGOR M", NumeroCartao = "123252341255", Validade = "10/10", CodSeguranca = 111, Valor = 10, BeneficiarioCNPJ="222"}

           };
            foreach (Doacao d in doacoes)
            {
                context.Doacoes.Add(d);
            }
            context.SaveChanges();
        }
    }
}


