using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechChallenge.Infra.Query;

namespace TechChallenge.Tests.Infra.Query
{
    public class ContatosQueryTests
    {
        [Fact]
        public void ListDDD_ShouldReturnCorrectQuery_WhenNoIdIsProvided()
        {
            // Arrange
            string expectedQuery = @"SELECT * FROM ""CodigosDeArea"" a";

            // Act
            string actualQuery = ContatosQuery.ListDDD();

            // Assert
            Assert.Equal(expectedQuery, actualQuery);
        }

        [Fact]
        public void ListDDD_ShouldReturnCorrectQuery_WhenIdIsProvided()
        {
            // Arrange
            int id = 5;
            string expectedQuery = @"SELECT * FROM ""CodigosDeArea"" a WHERE a.Id = @Id";

            // Act
            string actualQuery = ContatosQuery.ListDDD(id);

            // Assert
            Assert.Equal(expectedQuery, actualQuery);
        }


        [Fact]
        public void GetContatoByDDD_ShouldReturnCorrectQuery_WhenIdIsProvided()
        {
            // Arrange
            int id = 5;
            string expectedQuery = @"SELECT  * FROM ""Contatos""  where ""IdDDD"" = @Id";

            // Act
            string actualQuery = ContatosQuery.GetContatoByDDD(id);

            // Assert
            Assert.Equal(expectedQuery, actualQuery);
        }
    }
}
