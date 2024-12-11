using AutoMapper;
using TechChallenge.Domain.Model;
using TechChallenge.Domain.Model.DTO;
using TechChallenge.Domain.Model.ViewModel;

namespace TechChallenge.Domain.Config
{
    public class MappingConfig
    {
        public static MapperConfiguration RegisterMaps()
        {
            var MappingConfig = new MapperConfiguration(config =>
            {
                config.CreateMap<Contato, ContatoDto>().ReverseMap();
                config.CreateMap<ContatoInclusaoViewModel, Contato>()
                .ForMember(dest => dest.Ddd, opt => opt.Ignore())
                .ReverseMap();

                config.CreateMap<Usuario, UsuarioDTO>().ReverseMap();
                config.CreateMap<Usuario, UsuarioInclusaoViewModel>().ReverseMap();

            });
            return MappingConfig;
        }
    }
}
