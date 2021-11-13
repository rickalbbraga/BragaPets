using AutoMapper;
using BragaPets.Domain.DTOs.Responses;
using BragaPets.Domain.Entities;

namespace BragaPets.API.AutoMapper
{
    public class AutoMapperConfiguration : Profile
    {
        public AutoMapperConfiguration()
        {
            CreateMap<User, UserResponse>();

            // CreateMap<UserRequest, Teste>()
            //     .ForMember(d => d.Mensagem, opt =>
            //         opt.MapFrom(s => JsonConvert.DeserializeObject<TesteMensagem>(((JsonElement)s.Mensagem).GetRawText())));
        }
    }
}