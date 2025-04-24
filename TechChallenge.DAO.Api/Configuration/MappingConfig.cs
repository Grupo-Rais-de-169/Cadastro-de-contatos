using AutoMapper;
using TechChallenge.DAO.Api.Entities;
using TechChallenge.DAO.Api.ViewModel;

namespace TechChallenge.DAO.Domain.Config
{
    public static class MappingConfig
    {
        public static MapperConfiguration RegisterMaps()
        {
            var MappingConfig = new MapperConfiguration(config =>
            {
                config.CreateMap<ContatoInclusaoViewModel, Contato>()
                .ForMember(dest => dest.Ddd, opt => opt.Ignore())
                .ReverseMap();

                //config.CreateMap<Usuario, UsuarioInclusaoViewModel>().ReverseMap();

            });
            return MappingConfig;
        }

    }
}
