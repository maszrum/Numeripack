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
				new[] { "A", "B", "C" });
		}

		[Test]
		public void Test()
		{
			while (_permutationTree.MoveNext())
			{
				var currentElements = _permutationTree.Current.Elements;

				if (currentElements.Count == 1 && currentElements[0] == "A")
				{
					_permutationTree.Current.SkipBranch();
				}
				if (currentElements.Count == 2 && currentElements[0] == "C" && currentElements[1] == "A")
				{
					_permutationTree.Current.SkipBranch();
				}
				else
				{
					Console.WriteLine(string.Join(" -> ", _permutationTree.Current.Elements));
				}
			}

			Console.WriteLine("result:");

			var result = _permutationTree.GetResult();
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

			var result = _permutationTree.GetResult();

			Assert.Zero(result.Count);
		}
	}
}
