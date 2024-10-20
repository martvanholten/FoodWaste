using HotChocolate;
using HotChocolate.Data;
using HotChocolate.Types;
using System.Reflection.Metadata.Ecma335;

namespace FoodWasteAPI
{
    [QueryType]
    public class QueryDb
    {
        public string Hello(string name) => $"Hello, {name}";

        public IEnumerable<string> GetStrings()
        {
            string[] strings = new string[]
            {
                "test",
                "test2",
                "test3"
            };

            return strings;
        }


        [UseProjection]
        [UseFiltering]
        [UseSorting]
        public IEnumerable<Pakkage> Pakkages(PakkageService pakkageService)
            => pakkageService.GetPakkages()!;

        [UseProjection]
        [UseFiltering]
        [UseSorting]
        public Pakkage Pakkage(string title, PakkageService pakkageService) 
            => pakkageService.GetPakkage(title);
    }
}
