using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace Numeripack.Tests.Maths
{
    [TestFixture]
    internal class PositionalIncrementerGenericTests
    {
        [Test]
        public void Enumerate_Test()
        {
            const int baseNumber = 5;
            const int positions = 3;
            var incrementer = new PositionalIncrementer<int>(
                Enumerable.Range(0, baseNumber), positions);

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
            var incrementer = new PositionalIncrementer<int>(
                Enumerable.Range(0, baseNumber), positions);

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
        public void One_Position_Test()
        {
            var collection = new[] { 0.1, 0.2, 0.3, 0.4, 0.5 };

            var incrementer = new PositionalIncrementer<double>(collection, 1);

            var iList = new List<double>();
            foreach (var i in incrementer)
            {
                Assert.AreEqual(1, i.Count);
                iList.Add(i[0]);
            }

            CollectionAssert.AreEqual(collection, iList);
        }
    }
}