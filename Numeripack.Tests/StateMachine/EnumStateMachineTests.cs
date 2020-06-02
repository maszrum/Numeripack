using System;
using NUnit.Framework;

namespace Numeripack.Tests.StateMachine
{
    [TestFixture]
    internal class EnumStateMachineTests
    {
        private EnumStateMachine<ValidEnumOne> _validMachine;

        [SetUp]
        public void SetUp()
        {
            var builder = new EnumStateMachineBuilder<ValidEnumOne>();
            builder.For(ValidEnumOne.First)
                .AllowTransitionTo(ValidEnumOne.Fifth);
            builder.For(ValidEnumOne.Second)
                .AllowTransitionTo(ValidEnumOne.Third);
            builder.For(ValidEnumOne.Third)
                .AllowTransitionTo(ValidEnumOne.Fifth);
            builder.For(ValidEnumOne.Fourth)
                .AllowTransitionTo(ValidEnumOne.First);
            builder.For(ValidEnumOne.Fifth)
                .AllowTransitionTo(ValidEnumOne.Fourth)
                .AllowTransitionTo(ValidEnumOne.Third);

            _validMachine = builder.Build(ValidEnumOne.First);
        }

        [Test]
        public void Should_Throw_On_Invalid_Starting_State()
        {
            var builder = new EnumStateMachineBuilder<ValidEnumOne>();
            builder.For(ValidEnumOne.First)
                .AllowTransitionTo(ValidEnumOne.Fifth);
            builder.For(ValidEnumOne.Second)
                .AllowTransitionTo(ValidEnumOne.Third);
            builder.For(ValidEnumOne.Third)
                .AllowTransitionTo(ValidEnumOne.Fifth);
            builder.For(ValidEnumOne.Fourth)
                .AllowTransitionTo(ValidEnumOne.First);
            builder.For(ValidEnumOne.Fifth)
                .AllowTransitionTo(ValidEnumOne.Second);

            var exception = Assert.Throws<ArgumentException>(() =>
            {
                builder.Build((ValidEnumOne)3);
            });
            Assert.AreEqual("startingState", exception.ParamName);
        }

        [Test]
        public void Should_Throw_On_Duplicated_For()
        {
            var builder = new EnumStateMachineBuilder<ValidEnumOne>();
            builder.For(ValidEnumOne.First)
                .AllowTransitionTo(ValidEnumOne.Fifth);
            builder.For(ValidEnumOne.Second)
                .AllowTransitionTo(ValidEnumOne.Third);
            builder.For(ValidEnumOne.Third)
                .AllowTransitionTo(ValidEnumOne.Fifth);
            builder.For(ValidEnumOne.Fourth)
                .AllowTransitionTo(ValidEnumOne.First);
            builder.For(ValidEnumOne.Fifth)
                .AllowTransitionTo(ValidEnumOne.Second);
            builder.For(ValidEnumOne.Second)
                .AllowTransitionTo(ValidEnumOne.Third);

            var exception = Assert.Throws<InvalidOperationException>(() =>
            {
                builder.Build(ValidEnumOne.First);
            });
            Assert.IsTrue(exception.Message.Contains("duplication", StringComparison.Ordinal));
        }

        [Test]
        public void Should_Throw_On_Incomplete_Enum_Values()
        {
            var builder = new EnumStateMachineBuilder<ValidEnumOne>();
            builder.For(ValidEnumOne.First)
                .AllowTransitionTo(ValidEnumOne.Fifth);
            builder.For(ValidEnumOne.Second)
                .AllowTransitionTo(ValidEnumOne.Third);
            builder.For(ValidEnumOne.Third)
                .AllowTransitionTo(ValidEnumOne.Fifth);
            builder.For(ValidEnumOne.Fourth)
                .AllowTransitionTo(ValidEnumOne.First);

            var exception = Assert.Throws<InvalidOperationException>(() =>
            {
                builder.Build(ValidEnumOne.First);
            });
            Assert.IsTrue(exception.Message.Contains("unregistered", StringComparison.Ordinal));
        }

        [Test]
        public void Goto_State_Method_Test()
        {
            Assert.AreEqual(ValidEnumOne.First, _validMachine.CurrentState);
            _validMachine.GotoState(ValidEnumOne.Fifth);
            Assert.AreEqual(ValidEnumOne.Fifth, _validMachine.CurrentState);
            _validMachine.GotoState(ValidEnumOne.Fourth);
            Assert.AreEqual(ValidEnumOne.Fourth, _validMachine.CurrentState);
            _validMachine.GotoState(ValidEnumOne.First);
            Assert.AreEqual(ValidEnumOne.First, _validMachine.CurrentState);

            var invalidTransitions = new[] { ValidEnumOne.Second, ValidEnumOne.Third, ValidEnumOne.Fourth };

            foreach (var _ in invalidTransitions)
            {
                var ex1 = Assert.Throws<InvalidOperationException>(() =>
                {
                    _validMachine.GotoState(ValidEnumOne.Second);
                });
                Assert.IsTrue(ex1.Message.Contains("transition is not allowed", StringComparison.Ordinal));
            }

            var ex2 = Assert.Throws<InvalidOperationException>(() =>
            {
                _validMachine.GotoState(ValidEnumOne.First);
            });
            Assert.IsTrue(ex2.Message.Contains("is current state", StringComparison.Ordinal));
        }

        [Test]
        public void Current_State_Property_Get_Set_Test()
        {
            Assert.AreEqual(ValidEnumOne.First, _validMachine.CurrentState);
            _validMachine.CurrentState = ValidEnumOne.Fifth;
            Assert.AreEqual(ValidEnumOne.Fifth, _validMachine.CurrentState);
            _validMachine.CurrentState = ValidEnumOne.Fourth;
            Assert.AreEqual(ValidEnumOne.Fourth, _validMachine.CurrentState);
            _validMachine.CurrentState = ValidEnumOne.First;
            Assert.AreEqual(ValidEnumOne.First, _validMachine.CurrentState);

            var invalidTransitions = new[] { ValidEnumOne.Second, ValidEnumOne.Third, ValidEnumOne.Fourth };

            foreach (var _ in invalidTransitions)
            {
                var ex1 = Assert.Throws<InvalidOperationException>(() =>
                {
                    _validMachine.CurrentState = ValidEnumOne.Second;
                });
                Assert.IsTrue(ex1.Message.Contains("transition is not allowed", StringComparison.Ordinal));
            }

            var ex2 = Assert.Throws<InvalidOperationException>(() =>
            {
                _validMachine.CurrentState = ValidEnumOne.First;
            });
            Assert.IsTrue(ex2.Message.Contains("is current state", StringComparison.Ordinal));
        }

        [Test]
        public void State_Changed_Event_Test()
        {
            ValidEnumOne changedFrom = ValidEnumOne.Fifth;
            ValidEnumOne changedTo = ValidEnumOne.Fifth;
            _validMachine.StateChanged += (from, to) =>
            {
                changedFrom = from;
                changedTo = to;
            };

            _validMachine.GotoState(ValidEnumOne.Fifth);
            Assert.AreEqual(ValidEnumOne.First, changedFrom);
            Assert.AreEqual(ValidEnumOne.Fifth, changedTo);

            _validMachine.GotoState(ValidEnumOne.Fourth);
            Assert.AreEqual(ValidEnumOne.Fifth, changedFrom);
            Assert.AreEqual(ValidEnumOne.Fourth, changedTo);

            _validMachine.GotoState(ValidEnumOne.First);
            Assert.AreEqual(ValidEnumOne.Fourth, changedFrom);
            Assert.AreEqual(ValidEnumOne.First, changedTo);
        }

        [Test]
        public void Get_Allowed_Transitions_Test()
        {
            Assert.AreEqual(ValidEnumOne.First, _validMachine.CurrentState);

            CollectionAssert.AreEquivalent(
                new[] { ValidEnumOne.Fifth },
                _validMachine.GetAllowedTransitions());

            _validMachine.GotoState(ValidEnumOne.Fifth);

            CollectionAssert.AreEquivalent(
                new[] { ValidEnumOne.Fourth, ValidEnumOne.Third },
                _validMachine.GetAllowedTransitions());

            _validMachine.GotoState(ValidEnumOne.Fourth);

            CollectionAssert.AreEquivalent(
                new[] { ValidEnumOne.First },
                _validMachine.GetAllowedTransitions());
        }

        [Test]
        public void Is_Transition_Allowed_Test()
        {
            Assert.AreEqual(ValidEnumOne.First, _validMachine.CurrentState);

            Assert.IsFalse(_validMachine.IsTransitionAllowed(ValidEnumOne.First));
            Assert.IsFalse(_validMachine.IsTransitionAllowed(ValidEnumOne.Second));
            Assert.IsFalse(_validMachine.IsTransitionAllowed(ValidEnumOne.Third));
            Assert.IsFalse(_validMachine.IsTransitionAllowed(ValidEnumOne.Fourth));
            Assert.IsTrue(_validMachine.IsTransitionAllowed(ValidEnumOne.Fifth));

            _validMachine.GotoState(ValidEnumOne.Fifth);
            
            Assert.IsFalse(_validMachine.IsTransitionAllowed(ValidEnumOne.First));
            Assert.IsFalse(_validMachine.IsTransitionAllowed(ValidEnumOne.Second));
            Assert.IsTrue(_validMachine.IsTransitionAllowed(ValidEnumOne.Third));
            Assert.IsTrue(_validMachine.IsTransitionAllowed(ValidEnumOne.Fourth));
            Assert.IsFalse(_validMachine.IsTransitionAllowed(ValidEnumOne.Fifth));

            _validMachine.GotoState(ValidEnumOne.Fourth);

            Assert.IsTrue(_validMachine.IsTransitionAllowed(ValidEnumOne.First));
            Assert.IsFalse(_validMachine.IsTransitionAllowed(ValidEnumOne.Second));
            Assert.IsFalse(_validMachine.IsTransitionAllowed(ValidEnumOne.Third));
            Assert.IsFalse(_validMachine.IsTransitionAllowed(ValidEnumOne.Fourth));
            Assert.IsFalse(_validMachine.IsTransitionAllowed(ValidEnumOne.Fifth));
        }

        [Test]
        public void Implicit_Operator_T_Test()
        {
            Assert.IsTrue(ValidEnumOne.First == _validMachine);

            _validMachine.GotoState(ValidEnumOne.Fifth);

            Assert.IsTrue(ValidEnumOne.Fifth == _validMachine);

            _validMachine.GotoState(ValidEnumOne.Fourth);

            Assert.IsTrue(ValidEnumOne.Fourth == _validMachine);
        }
    }
}
