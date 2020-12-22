using System;
using System.Collections.Generic;

namespace CodingChallenge
{
    public class Size
    {
        public Guid Id { get; }

        public string Name { get; }

        private Size(Guid id, string name)
        {
            Id = id;
            Name = name;
        }


        public static Size Small = new Size(Guid.NewGuid(), "Small");
        public static Size Medium = new Size(Guid.NewGuid(), "Medium");
        public static Size Large = new Size(Guid.NewGuid(), "Large");


        public static List<Size> All = 
            new List<Size>
            {
                Small,
                Medium,
                Large
            };
    }
}