using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace Numeripack.Tests.Maths
{
    [TestFixture]
    internal class CollectionPermutatorTests
    {
        private string[] _identifiers;
        private int[] _elements;
        private CollectionPermutator<int, string> _permutator;

        [SetUp]
        public void SetUp()
        {
            _identifiers = new[] { "first", "second" };
            _elements = new[] { 1, 2, 3 };

            _permutator = new CollectionPermutator<int, string>(_elements, _identifiers);
        }

        [Test]
        public void Simple_Permutation_Test()
        {
            var permutations = new List<IReadOnlyList<IPermutedCollection<int, string>>>();

            while (_permutator.Permute())
            {
                var collections = _permutator.Current;
                permutations.Add(collections);

                CollectionAssert.AreEquivalent(
                    _identifiers, 
                    collections.Select(pc => pc.Identifier));

                CollectionAssert.AreEquivalent(
                    _elements,
                    collections.SelectMany(pc => pc));
            }

            var expectedPermutations = new List<Tuple<string, int[]>[]>()
            {
                new[]
                {
                    Tuple.Create("first", new[] {1, 2, 3}),
                    Tuple.Create("second", Array.Empty<int>())
                },
                new[]
                {
                    Tuple.Create("first", new[] {2, 3}),
                    Tuple.Create("second", new[] {1})
                },
                new[]
                {
                    Tuple.Create("first", new[] {3}),
                    Tuple.Create("second", new[] {1, 2})
                },
                new[]
                {
                    Tuple.Create("first", Array.Empty<int>()),
                    Tuple.Create("second", new[] {1, 2, 3})
                },
                new[]
                {
                    Tuple.Create("first", new[] {1, 3}),
                    Tuple.Create("second", new[] {2})
                },
                new[]
                {
                    Tuple.Create("first", new[] {1, 2}),
                    Tuple.Create("second", new[] {3})
                },
                new[]
                {
                    Tuple.Create("first", new[] {1}),
                    Tuple.Create("second", new[] {2, 3})
                },
                new[]
                {
                    Tuple.Create("first", new[] {2}),
                    Tuple.Create("second", new[] {1, 3})
                }
            };

            var perms = permutations.Select(p => 
                p.Select(pc => 
                    Tuple.Create(pc.Identifier, pc.ToArray()))
                    .ToArray())
                .ToArray();

            foreach (var expected in expectedPermutations)
            {
                var firstTuple = expected.Single(t => t.Item1 == "first");
                var secondTuple = expected.Single(t => t.Item1 == "second");

                var found = false;
                foreach (var p in perms)
                {
                    var p1 = p.Any(t => t.Item1 == "first" && t.Item2.SequenceEqual(firstTuple.Item2));
                    var p2 = p.Any(t => t.Item1 == "second" && t.Item2.SequenceEqual(secondTuple.Item2));

                    if (p1 && p2)
                    {
                        found = true;
                        break;
                    }
                }

                Assert.IsTrue(found);
            }
        }

        [Test]
        public void Combinations_Count_Test()
        {
            var count = 0;
            while (_permutator.Permute())
            {
                count++;
            }

            var expectedCombinations = _identifiers.Length.Pow(_elements.Length);

            Assert.AreEqual(expectedCombinations, count);
            Assert.AreEqual(expectedCombinations, _permutator.Combinations);
        }
    }
}
