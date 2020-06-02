using System;
using System.Collections.Generic;

namespace Numeripack
{
    public interface IStateMachine<T> where T : struct, Enum
    {
        T CurrentState { get; set; }

        void GotoState(T state);
        bool IsTransitionAllowed(T toState);
        IReadOnlyList<T> GetAllowedTransitions();
    }
}