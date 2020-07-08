using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Numeripack
{
	public class GenericPermutator<T> : IEnumerable<T[]>
	{
		private readonly List<T> _originalCollection;

		private T[] _value;
		private IEnumerator<T[]> _enumerator;

		public GenericPermutator(IEnumerable<T> collection)
		{
			if (collection == null)
			{
				throw new ArgumentNullException(nameof(collection));
			}

			_originalCollection = collection.ToList();

			if (!_originalCollection.Any())
			{
				throw new ArgumentException(
					"must contain at least one element");
			}

			Combinations = _originalCollection.Count.Factorial();

			Reset();
		}

		public int Combinations { get; }

		public IReadOnlyList<T> Elements => _originalCollection;

		private T[] NextPermutation(out bool reset)
		{
			var sourceArray = _enumerator.Current;

			Debug.Assert(sourceArray != null);

			var resultArray = new T[sourceArray.Length];
			Array.Copy(sourceArray, resultArray, sourceArray.Length);

			reset = false;

			if (!_enumerator.MoveNext())
			{
				Reset();
				reset = true;
			}

			return resultArray;
		}

		public T[] Permute()
		{
			return NextPermutation(out _);
		}

		public void Reset()
		{
			_value = _originalCollection.ToArray();

			_enumerator?.Dispose();

			_enumerator = HeapPermutation(_value).GetEnumerator();
			_enumerator.MoveNext();
		}

		public IEnumerator<T[]> GetEnumerator()
		{
			var permutation = NextPermutation(out bool reset);
			while (!reset)
			{
				yield return permutation;
				permutation = NextPermutation(out reset);
			}
			yield return permutation;
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
		
		private static IEnumerable<T[]> HeapPermutation(T[] array)
		{
			static void Swap(IList<T> arr, int firstIndex, int secondIndex)
			{
				var tmp = arr[firstIndex];
				arr[firstIndex] = arr[secondIndex];
				arr[secondIndex] = tmp;
			}

			var size = array.Length;
			var c = new int[size];

			yield return array;

			var i = 0;
			while (i < size)
			{
				if (c[i] < i)
				{
					Swap(
						arr: array, 
						firstIndex: i % 2 == 0 ? 0 : c[i], 
						secondIndex: i);

					yield return array;

					c[i]++;
					i = 0;
				}
				else
				{
					c[i] = 0;
					i++;
				}
			}
		}
	}
}
