using System;
using NUnit.Framework;

namespace Numeripack.Tests.Maths
{
    [TestFixture]
    internal class TreePermutatorTests
    {
        private TreePermutator<string> _permutationTree;

        [SetUp]
        public void SetUp()
        {
            _permutationTree = new TreePermutator<string>(
                new[] { "first", "second", "third", "fourth" });
        }

        [Test]
        public void Test()
        {
            while (_permutationTree.MoveNext())
            {
                Console.WriteLine(string.Join(" -> ", _permutationTree.Current.Elements));
                //var currentElements = permutationTree.Current.Elements;
                //if (currentElements.Count == 1 && currentElements[0] == 2)
                //    permutationTree.Current.SkipBranch();
                //if (currentElements.Count == 2 && currentElements[0] == 1 && currentElements[1] == 2)
                //    permutationTree.Current.SkipBranch();
            }

            Console.WriteLine("result:");

            var result = _permutationTree.Finish();
            foreach (var path in result)
            {
                Console.WriteLine(string.Join(" -> ", path));
            }
        }

        [Test]
        public void Skip_All()
        {
            while (_permutationTree.MoveNext())
            {
                _permutationTree.Current.SkipBranch();
            }

            var result = _permutationTree.Finish();

            Assert.Zero(result.Count);
        }
    }
}
