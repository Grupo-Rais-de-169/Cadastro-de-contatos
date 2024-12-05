using AutoMapper;
using TechChallenge.Domain.Model;
using TechChallenge.Domain.Model.ViewModel;

namespace TechChallenge.Domain.Config
{
    public class MappingConfig
    {
        public static MapperConfiguration RegisterMaps()
        {
            var MappingConfig = new MapperConfiguration(config =>
            {
                config.CreateMap<Contato, ContatoDTO>().ReverseMap();
                config.CreateMap<ContatoInclusaoViewModel, Contato>()
            // Ignorar o mapeamento da propriedade 'Ddd' (do tipo 'CodigoDeArea')
            .ForMember(dest => dest.Ddd, opt => opt.Ignore())
            .ReverseMap();
            });
            return MappingConfig;
        }
    }
}
