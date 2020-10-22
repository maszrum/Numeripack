using System;
using NUnit.Framework;

namespace Numeripack.Tests.Maths
{
    [TestFixture]
    internal class MinimalHamiltonianPathSeekerTests
    {
        public class PathPoint
        {
            public PathPoint(string name, int index)
            {
                Name = name;
                Index = index;
            }

            public string Name { get; set; }
            public int Index { get; set; }
        }

        [Test]
        public void Todo_Test()
        {
            var elements = new[]
            {
                new PathPoint("A", 0),
                new PathPoint("B", 1),
                new PathPoint("C", 2),
                new PathPoint("D", 3),
                new PathPoint("E", 4),
                new PathPoint("F", 5),
                new PathPoint("G", 6),
            };

            var costValues = new[,]
            {
                { 0, 2, 6, 4, 5, 8, 9 },
                { 2, 0, 3, 7, 1, 3, 3 },
                { 6, 3, 0, 3, 6, 1, 5 },
                { 4, 7, 3, 0, 8, 7, 6 },
                { 5, 1, 6, 8, 0, 4, 1 },
                { 8, 3, 1, 7, 4, 0, 4 },
                { 9, 3, 5, 6, 1, 4, 0 }
            };

            var seeker = new MinimalHamiltonianPathSeekerBuilder<PathPoint>()
                .ForElements(elements)
                .WithCostFunction((first, second, order) =>
                {
                    return costValues[first.Index, second.Index];
                })
                .Build();

            var result = seeker.FindResult();

            Console.WriteLine(result);
        }
    }
}
