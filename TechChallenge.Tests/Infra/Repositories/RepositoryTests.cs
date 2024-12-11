using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TechChallenge.Api.Controllers;
using TechChallenge.Domain;
using TechChallenge.Domain.Entities;
using TechChallenge.Infra.Context;
using TechChallenge.Infra.Repositories;
using Xunit;

namespace TechChallenge.Tests.Infra.Repositories
{
    public class RepositoryTests : IDisposable
    {
        private readonly MainContext _context;
        private readonly Repository<EntityBase> _repository;

        public RepositoryTests()
        {
            var options = new DbContextOptionsBuilder<MainContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new MainContext(options);
            _repository = new Repository<EntityBase>(_context);
        }

        public void Dispose()
        {
            _context?.Dispose();
        }

        private EntityBase CreateEntity(int id)
        {
            return new EntityBase { Id = id };
        }

        [Fact]
        public void Add_ShouldAddEntityAndSaveChanges()
        {
            // Arrange
            var entity = CreateEntity(1);

            // Act
            _repository.Add(entity);
            _context.SaveChanges();

            // Assert
            Assert.Contains(entity, _context.Set<EntityBase>());
        }

        [Fact]
        public async Task AddAsync_ShouldAddEntityAndSaveChangesAsync()
        {
            // Arrange
            var entity = CreateEntity(1);

            // Act
            await _repository.AddAsync(entity);
            await _context.SaveChangesAsync();

            // Assert
            Assert.Contains(entity, _context.Set<EntityBase>());
        }

        [Fact]
        public void Update_ShouldUpdateEntityAndSaveChanges()
        {
            var entity = CreateEntity(1);
            _context.Set<EntityBase>().Add(entity);
            _context.SaveChanges();

            // Act
            _context.Set<EntityBase>().Remove(entity);
            entity.Id = 2;
            _context.Set<EntityBase>().Add(entity);
            _context.SaveChanges();

            // Assert
            Assert.Equal(1, _context.Set<EntityBase>().First().Id);
        }

        [Fact]
        public void Delete_ShouldRemoveEntityAndSaveChanges()
        {
            // Arrange
            var entity = CreateEntity(1);
            _context.Set<EntityBase>().Add(entity);
            _context.SaveChanges();

            // Act
            _repository.Delete(1);
            _context.SaveChanges();

            // Assert
            Assert.DoesNotContain(entity, _context.Set<EntityBase>());
        }

        [Fact]
        public void GetById_ShouldReturnEntity()
        {
            // Arrange
            var entity = CreateEntity(1);
            _context.Set<EntityBase>().Add(entity);
            _context.SaveChanges();

            // Act
            var result = _repository.GetById(1);

            // Assert
            Assert.Equal(entity, result);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnEntity()
        {
            // Arrange
            var entity = CreateEntity(1);
            _context.Set<EntityBase>().Add(entity);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetByIdAsync(1);

            // Assert
            Assert.Equal(entity, result);
        }

        [Fact]
        public void GetAll_ShouldReturnAllEntities()
        {
            // Arrange
            var entities = new List<EntityBase>
        {
            CreateEntity(1),
            CreateEntity(2)
        };
            _context.Set<EntityBase>().AddRange(entities);
            _context.SaveChanges();

            // Act
            var result = _repository.GetAll();

            // Assert
            Assert.Equal(2, result.Count);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnAllEntities()
        {
            // Arrange
            var entities = new List<EntityBase>
        {
            CreateEntity(1),
            CreateEntity(2)
        };
            _context.Set<EntityBase>().AddRange(entities);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetAllAsync();

            // Assert
            Assert.Equal(2, result.Count);
        }
    }

}