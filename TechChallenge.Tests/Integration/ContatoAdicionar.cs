using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechChallenge.Domain;
using TechChallenge.Infra.Context;
using TechChallenge.Infra.Repositories;
using Xunit.Abstractions;

namespace TechChallenge.Tests.Integration
{
    [ExcludeFromCodeCoverage]
    public class ContatoAdicionar : IClassFixture<ContextoFixture>
    {
        private readonly MainContext _context;
        private readonly ITestOutputHelper _output;

        public ContatoAdicionar(ITestOutputHelper output, ContextoFixture fixture)
        {
            _context = fixture.Context;
            _output = output;
            _output.WriteLine(_context.GetHashCode().ToString());
        }

        [Fact]
        public void RegistraContatoNoBanco()
        {
            CodigoDeArea ddd = new CodigoDeArea()
            {
                Id = 11,
                Regiao = "São Paulo (Capital e Região Metropolitana)"
            };

            //arrange
            Contato contato = new Contato()
            {
                Nome = "São Paulo",
                Email = "Fortaleza",
                Telefone = "123456789",
                IdDDD = 11
            };

            var dalddd = new CodigoDeAreaRepository(_context);
            var dalContato = new ContatosRepository(_context, null);

            //act
            dalddd.Add(ddd);
            dalContato.Add(contato);

            //assert
            var contatoIncluido = dalContato.GetById(contato.Id);
            Assert.NotNull(contatoIncluido);
            Assert.Equal(contatoIncluido.Nome, contato.Nome);
        }

    }

}
