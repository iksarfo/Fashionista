using System;
using System.Collections.Generic;
using System.Linq;

namespace CodingChallenge
{
    public class SearchEngine
    {
        private readonly SearchResults[,] _results = new SearchResults[8, 32];

        [Flags]
        private enum Colors : short
        {
            Red = 1,
            Blue = 2,
            Yellow = 4,
            White = 8,
            Black = 16
        }

        [Flags]
        private enum Sizes : short
        {
            Small = 1,
            Medium = 2,
            Large = 4
        }

        private static IEnumerable<(short index, List<Size> sizes)> GetSizeOptions()
        {
            for (short bit = 1; bit < 8; bit++)
            {
                var current = (Sizes)bit;
                var sizes = (
                    from size in (Sizes[])Enum.GetValues(typeof(Sizes))
                    where (current & size) == size
                    select Size.All.Single(_ => _.Name == size.ToString())).ToList();

                yield return (bit, sizes);
            }
        }

        private static IEnumerable<(short index, List<Color> colors)> GetColorOptions()
        {
            for (short bit = 1; bit < 32; bit++)
            {
                var current = (Colors) bit;
                var colors = (
                    from color in (Colors[]) Enum.GetValues(typeof(Colors))
                    where (current & color) == color
                    select Color.All.Single(_ => _.Name == color.ToString())).ToList();

                yield return (bit, colors);
            }
        }

        public SearchEngine(List<Shirt> shirts)
        {
            var noSearchOptions = new SearchOptions();
            _results[0, 0] = GetResults(shirts, noSearchOptions);

            foreach (var (index, sizes) in GetSizeOptions())
            {
                var searchOptions = new SearchOptions { Sizes = sizes };
                _results[index, 0] = GetResults(shirts, searchOptions);
            }

            foreach (var (index, colors) in GetColorOptions())
            {
                var searchOptions = new SearchOptions { Colors = colors };
                _results[0, index] = GetResults(shirts, searchOptions);
            }

            foreach (var (sizeIndex, sizes) in GetSizeOptions())
            {
                foreach (var (colorIndex, colors) in GetColorOptions())
                {
                    var searchOptions = new SearchOptions { Sizes = sizes, Colors = colors };
                    _results[sizeIndex, colorIndex] = GetResults(shirts, searchOptions);
                }
            }
        }
        
        private static SearchResults GetResults(IEnumerable<Shirt> shirts, SearchOptions options)
        {
            var matchingShirts = shirts
                .Where(_ => (!options.Sizes.Any() || options.Sizes.Any(size => _.Size == size)) &&
                            (!options.Colors.Any() || options.Colors.Any(color => _.Color == color)))
                .Select(shirt => shirt)
                .ToList();

            var searchResults = new SearchResults
            {
                Shirts = matchingShirts,
                SizeCounts = matchingShirts.GroupBy(_ => _.Size).Select(_ => new SizeCount { Size = _.Key, Count = _.Count() }).ToList(),
                ColorCounts = matchingShirts.GroupBy(_ => _.Color).Select(_ => new ColorCount { Color = _.Key, Count = _.Count() }).ToList()
            };

            foreach (var size in Size.All.Where(size => searchResults.SizeCounts.All(_ => _.Size != size)))
            {
                searchResults.SizeCounts.Add(new SizeCount { Size = size, Count = 0 });
            }

            foreach (var color in Color.All.Where(color => searchResults.ColorCounts.All(_ => _.Color != color)))
            {
                searchResults.ColorCounts.Add(new ColorCount { Color = color, Count = 0 });
            }

            return searchResults;
        }

        public SearchResults Search(SearchOptions options)
        {
            if(options is null) throw new ArgumentNullException(nameof(options));

            return options.Sizes.Any() || options.Colors.Any()
                ? _results[GetSizeIndex(options), GetColorIndex(options)] ?? new SearchResults()
                : _results[0, 0];  
        }

        private static int GetColorIndex(SearchOptions options)
        {
            var colorBit = 0;
            var colorIndex = 0;
            foreach (var color in Color.All)
            {
                colorBit = colorBit == 0 ? 1 : colorBit * 2;
                if (options.Colors.Contains(color))
                    colorIndex += colorBit;
            }

            return colorIndex;
        }

        private static int GetSizeIndex(SearchOptions options)
        {
            var sizeBit = 0;
            var sizeIndex = 0;
            foreach (var size in Size.All)
            {
                sizeBit = sizeBit == 0 ? 1 : sizeBit * 2;
                if (options.Sizes.Contains(size))
                    sizeIndex += sizeBit;
            }

            return sizeIndex;
        }
    }
}