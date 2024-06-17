using AutoMapper;
using PersonAPI.Dtos;
using PersonAPI.Models;

namespace PersonAPI.Profiles
{
    public class PeopleProfile : Profile
    {
        public PeopleProfile()
        {
            // Source --> Target
            // CreateMap<Person, PersonReadDto>()
            // .ForMember(dest => dest.Age, opt => opt.MapFrom(src => src.YearsAlive));

            CreateMap<Person, PersonReadDto>()
            .ForMember(dest => dest.Age, opt => opt.MapFrom<AgeResolver>());

            // CreateMap<PersonCreateDto, Person>()
            // .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => $"{src.FirstName} {src.LastName}"));

            CreateMap<PersonCreateDto, Person>()
            .ForMember(dest => dest.FullName, opt => opt.MapFrom(new FullNameResolver()!))
            .ForMember(dest => dest.House, opt => opt.NullSubstitute("No House"));


            CreateMap<PersonUpdateDto, Person>()
            .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => $"{src.FirstName} {src.LastName}"));

            CreateMap<string, int>().ConvertUsing<IntTypeConverter>();

        }
    }
}