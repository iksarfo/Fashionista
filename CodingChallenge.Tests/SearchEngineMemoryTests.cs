using System;
using System.Collections.Generic;
using System.Diagnostics;
using CodingChallenge.Tests.SampleData;
using NUnit.Framework;

namespace CodingChallenge.Tests
{
    [TestFixture]
    public class SearchEngineMemoryTests : SearchEngineTestsBase
    {
        private List<Shirt> _shirts;
        private SearchEngine _searchEngine;

        [SetUp]
        public void Setup()
        {
            var dataBuilder = new SampleDataBuilder(1000000);

            _shirts = dataBuilder.CreateShirts();

            _searchEngine = new SearchEngine(_shirts);
        }

        [Test]
        [Ignore("This slows down entire test suite but remains for convenience")]
        public void MemoryTest()
        {
            var sw = new Stopwatch();
            sw.Start();

            var options = new SearchOptions
            {
                Colors = new List<Color> { Color.Red, Color.Black, Color.Blue, Color.White, Color.Yellow },
                Sizes = new List<Size> { Size.Small, Size.Medium, Size.Large }
            };

            var results = _searchEngine.Search(options);

            sw.Stop();
            Console.WriteLine($"Test fixture finished in {sw.ElapsedMilliseconds} milliseconds");

            AssertResults(results.Shirts, options);
            AssertSizeCounts(_shirts, options, results.SizeCounts);
            AssertColorCounts(_shirts, options, results.ColorCounts);
        }
    }
}
