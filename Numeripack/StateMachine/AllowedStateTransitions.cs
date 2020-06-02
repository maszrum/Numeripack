using System;
using System.Collections.Generic;
using System.Linq;

namespace Numeripack
{
	public class AllowedStateTransitions<T>
		where T : struct, Enum
	{
		private static readonly Lazy<HashSet<Type>> _verifiedTypes 
			= new Lazy<HashSet<Type>>(() => new HashSet<Type>());

		private readonly Type _enumType;

		public AllowedStateTransitions(T originState, T[] allowedStates)
		{
			_enumType = typeof(T);

			ThrowIfEnumTypeIsInvalid();

			OriginState = originState;
			AllowedStates = allowedStates ?? throw new ArgumentNullException(nameof(allowedStates));

			ThrowIfDuplicatedAllowedState();

			ThrowIfSpecifiedInvalidArguments();
		}

		public T OriginState { get; }

		public IReadOnlyList<T> AllowedStates { get; }

		private void ThrowIfDuplicatedAllowedState()
		{
			if (AllowedStates.Distinct().Count() != AllowedStates.Count)
			{
				throw new ArgumentException(
					$"duplicated transition for origin state {OriginState}", nameof(AllowedStates));
			}
		}

		public bool IsTransitionAllowed(T toState)
		{
			if (!Enum.IsDefined(_enumType, toState))
			{
				throw new ArgumentException(
					$"value {toState} is not defined in enum: {_enumType.Name}");
			}

			return AllowedStates.Contains(toState);
		}

		private void ThrowIfSpecifiedInvalidArguments()
		{
			if (AllowedStates.Count == 0)
			{
				throw new InvalidOperationException(
					"at least one allowed transition should be specified");
			}

			if (AllowedStates.Contains(OriginState))
			{
				throw new ArgumentException($"cannot define transition from {OriginState} to {OriginState}");
			}

			if (!Enum.IsDefined(_enumType, OriginState)
				|| !AllowedStates.All(s => Enum.IsDefined(_enumType, s)))
			{
				throw new ArgumentException(
					$"value {OriginState} is not defined in enum: {_enumType.Name}");
			}
		}

		private void ThrowIfEnumTypeIsInvalid()
		{
			if (!_verifiedTypes.Value.Contains(_enumType))
			{
				var enumValues = (int[])Enum.GetValues(_enumType);
				var expectedValue = enumValues[0];
				foreach (var enumValue in enumValues)
				{
					if (expectedValue != enumValue || enumValues[0] < 0)
					{
						throw new ArgumentException(
							$"invalid enum {_enumType.Name}: values should be consecutive multiples of 2", nameof(T));
					}
					expectedValue *= 2;
				}
				_verifiedTypes.Value.Add(_enumType);
			}
		}
	}
}
