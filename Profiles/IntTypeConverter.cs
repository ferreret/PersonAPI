using AutoMapper;

namespace PersonAPI.Profiles
{
    public class IntTypeConverter : ITypeConverter<string, int>
    {
        public int Convert(string source, int destination, ResolutionContext context)
        {
            var converterInt = 1;

            if (int.TryParse(source, out int result))
            {
                converterInt = result;
            }
            else
            {
                Console.WriteLine($"Whoops! {source} is not a valid integer. Defaulting to {converterInt}.");
            }

            return converterInt;
        }
    }
}