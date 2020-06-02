using System;
using System.Collections.Generic;
using System.Linq;

namespace Numeripack
{
    public class EnumStateMachineBuilder<T>
        where T : struct, Enum
    {
        private readonly List<AllowedStateTransitions<T>> _allowedTransitions = 
            new List<AllowedStateTransitions<T>>();

        private T _forState;
        private HashSet<T> _transitions;

        public EnumStateMachineBuilderFor<T> For(T state)
        {
            if (_transitions != null)
            {
                FinishAllowedTransitionAndAddToList();
            }

            _forState = state;
            _transitions = new HashSet<T>();

            return new EnumStateMachineBuilderFor<T>(AllowTransitionTo);
        }

        private void FinishAllowedTransitionAndAddToList()
        {
            var ast = new AllowedStateTransitions<T>(_forState, _transitions.ToArray());
            _allowedTransitions.Add(ast);
        }

        public EnumStateMachine<T> Build(T startingState)
        {
            FinishAllowedTransitionAndAddToList();

            return new EnumStateMachine<T>(_allowedTransitions, startingState);
        }

        private void AllowTransitionTo(T state)
        {
            _transitions.Add(state);
        }
    }

    public class EnumStateMachineBuilderFor<T>
        where T : struct, Enum
    {
        private readonly Action<T> _addTransitionAction;

        public EnumStateMachineBuilderFor(Action<T> addTransitionAction)
        {
            _addTransitionAction = addTransitionAction ?? throw new ArgumentNullException(nameof(addTransitionAction));
        }

        public EnumStateMachineBuilderFor<T> AllowTransitionTo(T state)
        {
            _addTransitionAction(state);
            return this;
        }
    }
}
