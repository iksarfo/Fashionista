using System;
using System.Collections.Generic;
using System.Linq;

namespace CodingChallenge.Tests.SampleData
{
    public class SampleDataBuilder
    {
        private readonly int _numberOfShirts;

        private readonly Random _random = new Random();

        
        public SampleDataBuilder(int numberOfShirts)
        {
            _numberOfShirts = numberOfShirts;

        }


        public List<Shirt> CreateShirts()
        {
            return Enumerable.Range(0, _numberOfShirts)
                .Select(i => new Shirt(Guid.NewGuid(), $"Shirt {i}", GetRandomSize(), GetRandomColor()))
                .ToList();
        }

       
        private Size GetRandomSize()
        {
            
            var sizes = Size.All;
            var index = _random.Next(0, sizes.Count);
            return sizes.ElementAt(index);
        }


        private Color GetRandomColor()
        {
            var colors = Color.All;
            var index = _random.Next(0, colors.Count);
            return colors.ElementAt(index);
        }
    }
}