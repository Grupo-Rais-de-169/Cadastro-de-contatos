using MassTransit;
using Microsoft.Extensions.Options;
using Moq;
using TechChallenge.Cadastro.Api.Configuration;

namespace TechChallenge.IntegrationTests
{
    public class ContatoServiceFixture
    {
        public IOptions<MassTransitConfig> Config { get; } = Options.Create(new MassTransitConfig());
        public IBus Bus { get; } = new Mock<IBus>().Object;
    }
}
