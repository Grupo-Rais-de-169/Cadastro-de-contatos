using Microsoft.EntityFrameworkCore;
using Moq;
using TechChallenge.Domain;
using TechChallenge.Infra.Context;
using TechChallenge.Infra.Repositories;

namespace TechChallenge.Tests.Infra.Repositories
{
    public class CodigoDeAreaRepositoryTests
    {
        private readonly Mock<IDbContextFactory<MainContext>> _dbContextFactory;

        public CodigoDeAreaRepositoryTests()
        {
            _dbContextFactory = new Mock<IDbContextFactory<MainContext>>();
        }

        private MainContext GetInMemoryContext()
        {
            var options = new DbContextOptionsBuilder<MainContext>()
             .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
             .Options;
            return new MainContext(options, _dbContextFactory.Object);
        }

        private CodigoDeAreaRepository CreateRepository()
        {
            var context = GetInMemoryContext();
            return new CodigoDeAreaRepository(context);
        }

        private CodigoDeArea GetSampleCodigoDeArea()
        {
            return new CodigoDeArea
            {
                Regiao = "Centro-Oeste",
                Contatos = new List<Contato>
            {
                new Contato { Nome = "Pedro", Email = "pedro@email.com", Telefone = "246810123", IdDDD = 1 },
                new Contato { Nome = "Sofia", Email = "sofia@email.com", Telefone = "987654321", IdDDD = 1 }
            }
            };
        }

        [Fact]
        public async Task Add_ShouldAddCodigoDeAreaWithContacts()
        {
            //Arrange
            using var context = GetInMemoryContext();
            var repository = CreateRepository();
            var codigoDeArea = GetSampleCodigoDeArea();
            await context.CodigosDeArea.AddAsync(codigoDeArea);
            await context.SaveChangesAsync();

            //Act
            var result = await context.CodigosDeArea.Include(c => c.Contatos).FirstOrDefaultAsync();

            //Assert
            Assert.Equal("Centro-Oeste", result.Regiao);
            Assert.NotNull(result);
            Assert.NotEmpty(result.Contatos);
            Assert.Equal(2, result.Contatos.Count);
        }

        [Fact]
        public async Task GetById_ShouldReturnCodigoDeAreaWithContacts_WhenExists()
        {
            // Arrange
            using var context = GetInMemoryContext();
            var repository = CreateRepository();
            var codigoDeArea = GetSampleCodigoDeArea();
            await context.CodigosDeArea.AddAsync(codigoDeArea);
            await context.SaveChangesAsync();

            //Act
            var result = await context.CodigosDeArea.Where(ca=>ca.Id == 1).FirstOrDefaultAsync(); 
           
            // Assert
            Assert.NotNull(result);
            Assert.Equal("Centro-Oeste", result.Regiao);
            Assert.NotEmpty(result.Contatos);
        }

        
    }


}
