using System;
using System.Collections.Generic;
using System.Linq;

namespace Numeripack
{
	public class MinimalHamiltonianPathSeeker<T>
	{
		private readonly ExtremumIterator<TreePermutator<T>, IPermutationPath<T>> _seeker;
		private readonly Func<T, T, int, double> _costFunction;
		private readonly double[] _costs;
		private int _argumentCounter;

		public MinimalHamiltonianPathSeeker(
			IEnumerable<T> elements, Func<T, T, int, double> costFunction)
		{
			_costFunction = costFunction ?? throw new ArgumentNullException(nameof(costFunction));

			var elementsArr = elements as T[] ?? elements.ToArray();

			_costs = new double[elementsArr.Count() - 1];

			var permutator = new TreePermutator<T>(elementsArr);
			permutator.MoveNext();

			_seeker = new ExtremumIteratorBuilder()
				.Find(Extremum.Minimum)
				.ForObject(permutator)
				.SelectArgument(p => p.Current)
				.WithStep(Step)
				.WithArgumentDomain(pp => pp.IsFull)
				.EndsIf(ctx => !ctx.Object.MoveNext())
				.Build();
		}

		public ExtremumValue<IPermutationPath<T>> Current => _seeker.Current;

		public bool Finished => _seeker.Finished;

		public int StepNumber => _seeker.StepNumber;

		public ExtremumValue<IPermutationPath<T>> FindResult()
		{
			return _seeker.FindResult();
		}

		public bool Next()
		{
			return _seeker.Next();
		}

		private double Step(IStepContext<TreePermutator<T>, IPermutationPath<T>> context)
		{
			var elements = context.Argument.Elements;
			var argumentsCount = context.Argument.Elements.Count;
			var argumentIndex = argumentsCount - 1;

			if (argumentsCount >= 2)
			{
				var from = argumentIndex == 0 ? default : elements[argumentIndex - 1];
				var to = elements[argumentIndex];
				_costs[argumentIndex - 1] = _costFunction(from, to, argumentIndex);

				for (var i = argumentsCount; i < _argumentCounter; i++)
				{
					_costs[i] = 0;
				}

				_argumentCounter = argumentIndex;

				var cost = _costs.Sum();

				if (context.CurrentExtremum != null &&
				    !_seeker.ExtremumComparer.ShouldBeNewCurrent(context.CurrentExtremum.Value, cost))
				{
					context.Argument.SkipBranch();
				}

				return cost;
			}

			return 0;
		}
	}
}
