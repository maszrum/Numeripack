using System;
using System.Collections.Generic;

namespace Numeripack
{
	internal class PermutationPath<T> : IPermutationPath<T>
	{
		private readonly Action<int> _quitPathAction;

		public PermutationPath(List<T> elements, bool isFull, Action<int> quitPathAction)
		{
			ElementsRw = elements ?? throw new ArgumentNullException(nameof(elements));

			if (elements.Count == 0)
			{
				throw new ArgumentException(
					"must contain at least one element", nameof(elements));
			}

			IsFull = isFull;
			_quitPathAction = quitPathAction ?? throw new ArgumentNullException(nameof(quitPathAction));
		}

		public List<T> ElementsRw { get; }

		public IReadOnlyList<T> Elements => ElementsRw;

		public bool IsFull { get; }

		public void SkipBranch()
		{
			_quitPathAction(ElementsRw.Count - 1);
		}

		public override string ToString()
			=> $"( {string.Join(" -> ", Elements)} )";
	}
}
