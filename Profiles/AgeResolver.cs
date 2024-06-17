using AutoMapper;
using AutoMapper.Execution;
using PersonAPI.Dtos;
using PersonAPI.Models;

namespace PersonAPI.Profiles
{
    public class AgeResolver : IValueResolver<Person, PersonReadDto, int>
    {
        public int Resolve(Person source, PersonReadDto destination, int destMember, ResolutionContext context)
        {
            var today = DateTime.Today;
            var splitDoB = source.DoB!.Split("-");

            return today.Year - int.Parse(splitDoB[0]) + 100;
        }
    }
}