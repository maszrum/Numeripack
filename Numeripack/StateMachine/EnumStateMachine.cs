using System;
using System.Collections.Generic;
using System.Linq;

namespace Numeripack
{
    public delegate void StateMachineStateChanged<in T>(T from, T to);

    public class EnumStateMachine<T> where T : struct, Enum
    {
        public event StateMachineStateChanged<T> StateChanged;

        private readonly Dictionary<T, AllowedStateTransitions<T>> _allowedTransitions;

        private T _currentState;

        public EnumStateMachine(List<AllowedStateTransitions<T>> allowedTransitions, T startingState)
        {
            if (allowedTransitions == null)
            {
                throw new ArgumentNullException(nameof(allowedTransitions));
            }

            ThrowIfStartingStateIsNotDefinedInEnum(startingState);

            ThrowIfDuplicatedTransitions(allowedTransitions);

            ThrowIfAnyEnumValuesWasNotRegistered(allowedTransitions);

            _allowedTransitions = allowedTransitions.ToDictionary(at => at.OriginState);
            _currentState = startingState;
        }

        public T CurrentState
        {
            get => _currentState;
            set => GotoState(value);
        }

        public void GotoState(T state)
        {
            if (state.Equals(_currentState))
            {
                throw new InvalidOperationException("specified state is current state");
            }

            if (!IsTransitionAllowed(state))
            {
                throw new InvalidOperationException(
                    $"specified transition is not allowed: {_currentState} -> {state}");
            }

            var previousState = _currentState;
            _currentState = state;

            StateChanged?.Invoke(previousState, state);
        }

        public bool IsTransitionAllowed(T toState)
            => _allowedTransitions[_currentState].IsTransitionAllowed(toState);

        public IReadOnlyList<T> GetAllowedTransitions()
            => _allowedTransitions[_currentState].AllowedStates;

        private static void ThrowIfAnyEnumValuesWasNotRegistered(IEnumerable<AllowedStateTransitions<T>> allowedTransitions)
        {
            var type = typeof(T);

            var enumValues = (T[])Enum.GetValues(type);
            var registeredValues = allowedTransitions.Select(at => at.OriginState);
            var missingValues = enumValues
                .Where(v => !registeredValues.Contains(v))
                .ToArray();

            if (missingValues.Length > 0)
            {
                throw new InvalidOperationException(
                    $"there are unregistered values of enum type {type.Name}: {string.Join(", ", missingValues)}");
            }
        }

        private static void ThrowIfStartingStateIsNotDefinedInEnum(T startingState)
        {
            var type = typeof(T);
            if (!Enum.IsDefined(type, startingState))
            {
                throw new ArgumentException(
                    $"specified value is not defined in enum {type.Name}: {startingState}", nameof(startingState));
            }
        }

        private static void ThrowIfDuplicatedTransitions(IEnumerable<AllowedStateTransitions<T>> allowedTransitions)
        {
            var originStates = allowedTransitions
                .Select(at => at.OriginState)
                .ToArray();

            var originStatesCount = originStates.Count();
            var uniqueOriginStatesCount = originStates.Distinct().Count();
            if (originStatesCount != uniqueOriginStatesCount)
            {
                throw new InvalidOperationException($"detected duplication of transitions set");
            }
        }

        public static implicit operator T(EnumStateMachine<T> stateMachine)
        {
            return stateMachine.CurrentState;
        }
    }
}
