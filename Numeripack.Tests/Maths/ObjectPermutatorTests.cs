using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace Numeripack.Tests.Maths
{
	[TestFixture]
	internal class ObjectPermutatorTests
	{
		[Test]
		public void Foreach_Test()
		{
			var permutator = new GenericPermutator<int>(new[] { 1, 2, 3 });

			var list = new List<int[]>();

			var count = 0;
			foreach (var permutation in permutator)
			{
				list.Add(permutation);
				count++;
			}

			Assert.AreEqual(permutator.Combinations, count);

			var expectedPermutations = new List<int[]>()
			{
				new[] { 1, 2, 3 },
				new[] { 1, 3, 2 },
				new[] { 2, 3, 1 },
				new[] { 2, 1, 3 },
				new[] { 3, 1, 2 },
				new[] { 3, 2, 1 }
			};

			foreach (var p in list)
			{
				var foundPermutation = expectedPermutations.SingleOrDefault(
					i => i.SequenceEqual(p));

				Assert.IsNotNull(foundPermutation);
			}
		}

		[Test]
		public void Permute_Method_Test()
		{
			var permutator = new GenericPermutator<int>(new[] { 1, 2, 3 });

			var list = permutator.ToList();

			for (var i = 0; i < permutator.Combinations; i++)
			{
				CollectionAssert.AreEqual(list[i], permutator.Permute());
			}
		}

		[Test]
		public void Readme_Example()
		{
			var elements = new[] { "one", "two", "three" };
			var permutator = new GenericPermutator<string>(elements);

			foreach (var permutation in permutator)
			{
				Console.WriteLine(string.Join(", ", permutation));
			}
		}
	}
}
