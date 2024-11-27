using AutoMapper;
using TechChallenge.Domain.Model;

namespace TechChallenge.Domain.Config
{
    public class MappingConfig
    {
        public static MapperConfiguration RegisterMaps()
        {
            var MappingConfig = new MapperConfiguration(config =>
            {
                config.CreateMap<Contato, ContatoDTO>().ReverseMap();
            });
            return MappingConfig;
        }
    }
}
