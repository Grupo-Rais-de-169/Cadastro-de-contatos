using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechChallenge.Domain;
using TechChallenge.Infra.Context;
using TechChallenge.Infra.Repositories;

namespace TechChallenge.Tests.Infra.Repositories
{
    using Microsoft.EntityFrameworkCore;
    using Xunit;
    using System.Linq;
    using System.Threading.Tasks;

    public class CodigoDeAreaRepositoryTests
    {
        //private MainContext GetInMemoryContext()
        //{
        //    var options = new DbContextOptionsBuilder<MainContext>()
        //        .UseInMemoryDatabase(databaseName: "TestDatabase")
        //        .Options;

        //    return new MainContext(options);
        //}

        //[Fact]
        //public async Task Add_ShouldAddCodigoDeAreaWithContacts()
        //{
        //    // Arrange
        //    using var context = GetInMemoryContext();
        //    var repository = new CodigoDeAreaRepository(context);

        //    var codigoDeArea = new CodigoDeArea
        //    {
        //        Regiao = "Sudeste",
        //        Contatos = new List<Contato>
        //    {
        //        new Contato { Nome = "João", Email = "joao@email.com", Telefone = "123456789", IdDDD = 1 },
        //        new Contato { Nome = "Maria", Email = "maria@email.com", Telefone = "987654321", IdDDD = 1 }
        //    }
        //    };

        //    // Act
        //    await repository.AddAsync(codigoDeArea);
        //    await context.SaveChangesAsync();

        //    // Assert
        //    var result = await context.CodigoDeAreas
        //        .Include(c => c.Contatos)
        //        .FirstOrDefaultAsync(c => c.Regiao == "Sudeste");

        //    Assert.NotNull(result);
        //    Assert.Equal("Sudeste", result.Regiao);
        //    Assert.NotEmpty(result.Contatos);
        //    Assert.Equal(2, result.Contatos.Count);
        //}

        //[Fact]
        //public async Task GetById_ShouldReturnCodigoDeAreaWithContacts_WhenExists()
        //{
        //    // Arrange
        //    using var context = GetInMemoryContext();
        //    var repository = new CodigoDeAreaRepository(context);

        //    var codigoDeArea = new CodigoDeArea
        //    {
        //        Id = 1,
        //        Regiao = "Norte",
        //        Contatos = new List<Contato>
        //    {
        //        new Contato { Nome = "Ana", Email = "ana@email.com", Telefone = "555555555", IdDDD = 1 }
        //    }
        //    };

        //    context.CodigoDeAreas.Add(codigoDeArea);
        //    await context.SaveChangesAsync();

        //    // Act
        //    var result = await repository.GetByIdAsync(1);

        //    // Assert
        //    Assert.NotNull(result);
        //    Assert.Equal("Norte", result.Regiao);
        //    Assert.NotEmpty(result.Contatos);
        //    Assert.Single(result.Contatos);
        //}

        //[Fact]
        //public async Task Delete_ShouldRemoveCodigoDeAreaAndCascadeDeleteContacts()
        //{
        //    // Arrange
        //    using var context = GetInMemoryContext();
        //    var repository = new CodigoDeAreaRepository(context);

        //    var codigoDeArea = new CodigoDeArea
        //    {
        //        Id = 1,
        //        Regiao = "Sul",
        //        Contatos = new List<Contato>
        //    {
        //        new Contato { Nome = "Carlos", Email = "carlos@email.com", Telefone = "777777777", IdDDD = 1 }
        //    }
        //    };

        //    context.CodigoDeAreas.Add(codigoDeArea);
        //    await context.SaveChangesAsync();

        //    // Act
        //    await repository.DeleteAsync(codigoDeArea);
        //    await context.SaveChangesAsync();

        //    // Assert
        //    var result = await context.CodigoDeAreas.FirstOrDefaultAsync(c => c.Id == 1);
        //    Assert.Null(result);

        //    var relatedContacts = context.Contatos.Where(c => c.IdDDD == 1).ToList();
        //    Assert.Empty(relatedContacts);
        //}
    }


}
