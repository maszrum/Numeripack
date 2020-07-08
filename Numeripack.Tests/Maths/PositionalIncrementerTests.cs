using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace Numeripack.Tests.Maths
{
	[TestFixture]
	internal class PositionalIncrementerTests
	{
		[Test]
		public void Enumerate_Test()
		{
			const int baseNumber = 5;
			const int positions = 3;
			var incrementer = new PositionalIncrementer(baseNumber, positions);

			var values = new List<int>[positions];
			for (var i = 0; i < positions; i++)
			{
				values[i] = new List<int>();
			}

			var index = 0;
			foreach (var value in incrementer)
			{
				for (var i = 0; i < positions; i++)
				{
					values[i].Add(value[i]);
				}
				Assert.AreEqual(index % (baseNumber), value[0]);
				index++;
			}

			CollectionAssert.AreEqual(
				Enumerable.Repeat(Enumerable.Range(0, baseNumber), baseNumber).SelectMany(v => v),
				values[1].SkipRepetitions());

			CollectionAssert.AreEqual(
				Enumerable.Range(0, baseNumber),
				values[2].SkipRepetitions());

			Assert.AreEqual(baseNumber.Pow(positions), index);
			Assert.AreEqual(index, incrementer.Combinations);
		}

		[Test]
		public void Is_Zero_Test()
		{
			const int baseNumber = 5;
			const int positions = 3;
			var incrementer = new PositionalIncrementer(baseNumber, positions);

			Assert.IsTrue(incrementer.IsZero);

			incrementer.Increment();
			Assert.IsFalse(incrementer.IsZero);
			incrementer.Increment();
			Assert.IsFalse(incrementer.IsZero);

			CollectionAssert.AreEqual(new[] { 2, 0, 0 }, incrementer.Values);

			var index = incrementer.Count();

			Assert.AreEqual(baseNumber.Pow(positions) - 2, index);
			Assert.AreEqual(index + 2, incrementer.Combinations);

			Assert.IsTrue(incrementer.IsZero);
		}

		[Test]
		public void Readme_Example_First()
		{
			const int baseNumber = 3;
			const int positions = 4;
			var incrementer = new PositionalIncrementer(baseNumber, positions);

			foreach (var value in incrementer)
			{
				Console.WriteLine(string.Join(" ", value));
			}
		}

		[Test]
		public void Readme_Example_Second()
		{
			var elements = new[] { "one", "two", "three", "four" };
			const int positions = 4;

			var incrementer = new PositionalIncrementer<string>(elements, positions);

			foreach (var value in incrementer)
			{
				Console.WriteLine(string.Join(" ", value));
			}
		}
	}
}