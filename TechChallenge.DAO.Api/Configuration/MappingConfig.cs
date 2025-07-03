using AutoMapper;
using TechChallenge.Core.ViewModels;
using TechChallenge.DAO.Api.Entities;

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
