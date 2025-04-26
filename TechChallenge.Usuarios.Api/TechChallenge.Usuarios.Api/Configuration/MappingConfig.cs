using AutoMapper;
using TechChallenge.Usuarios.Api.Domain.Entities;
using TechChallenge.Usuarios.Api.ViewModels;

namespace TechChallenge.Usuarios.Api.Configuration
{
    public static class MappingConfig
    {
        public static MapperConfiguration RegisterMaps()
        {
            var MappingConfig = new MapperConfiguration(config =>
            {
                //config.CreateMap<ContatoInclusaoViewModel, Contato>()
                //.ForMember(dest => dest.Ddd, opt => opt.Ignore())
                //.ReverseMap();

                config.CreateMap<Usuario, UsuarioInclusaoViewModel>().ReverseMap();

            });
            return MappingConfig;
        }

    }
}
