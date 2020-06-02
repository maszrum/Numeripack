using System;
using NUnit.Framework;

namespace Numeripack.Tests.StateMachine
{
    [TestFixture]
    internal class AllowedStateTransitionTests
    {
        [Test]
        public void Should_Throw_If_Invalid_Type()
        {
            var allowedTransitionsOne = new AllowedStateTransitions<ValidEnumOne>(
                ValidEnumOne.Fifth,
                new[] { ValidEnumOne.First, ValidEnumOne.Second });
            Console.WriteLine(allowedTransitionsOne);

            var allowedTransitionsTwo = new AllowedStateTransitions<ValidEnumTwo>(
                ValidEnumTwo.Fifth,
                new[] { ValidEnumTwo.First, ValidEnumTwo.Second });
            Console.WriteLine(allowedTransitionsTwo);

            var firstException = Assert.Throws<ArgumentException>(() =>
            {
                var allowedTransitions = new AllowedStateTransitions<InvalidEnumOne>(
                    InvalidEnumOne.Fifth,
                    new[] { InvalidEnumOne.First, InvalidEnumOne.Second });
                Console.WriteLine(allowedTransitions);
            });
            Assert.IsTrue(firstException.Message.Contains("multiples", StringComparison.Ordinal));

            var secondException = Assert.Throws<ArgumentException>(() =>
            {
                var allowedTransitions = new AllowedStateTransitions<InvalidEnumTwo>(
                    InvalidEnumTwo.Fifth,
                    new[] { InvalidEnumTwo.First, InvalidEnumTwo.Second });
                Console.WriteLine(allowedTransitions);
            });
            Assert.IsTrue(secondException.Message.Contains("multiples", StringComparison.Ordinal));
        }

        [Test]
        public void Should_Throw_If_Invalid_Enum_Passed()
        {
            var firstException = Assert.Throws<ArgumentException>(() =>
            {
                var allowedTransitions = new AllowedStateTransitions<ValidEnumOne>(
                    ValidEnumOne.Fifth,
                    new[] { ValidEnumOne.First, ValidEnumOne.Second, (ValidEnumOne)3 });
                Console.WriteLine(allowedTransitions);
            });
            Assert.IsTrue(firstException.Message.Contains("is not defined", StringComparison.Ordinal));

            var secondException = Assert.Throws<ArgumentException>(() =>
            {
                var allowedTransitions = new AllowedStateTransitions<ValidEnumOne>(
                    (ValidEnumOne)3,
                    new[] { ValidEnumOne.First, ValidEnumOne.Second });
                Console.WriteLine(allowedTransitions);
            });
            Assert.IsTrue(secondException.Message.Contains("is not defined", StringComparison.Ordinal));
        }

        [Test]
        public void Should_Throw_On_T_To_T_Transition()
        {
            var exception = Assert.Throws<ArgumentException>(() =>
            {
                var allowedTransitions = new AllowedStateTransitions<ValidEnumOne>(
                    ValidEnumOne.Fifth,
                    new[] { ValidEnumOne.Fifth, ValidEnumOne.Second });
                Console.WriteLine(allowedTransitions);
            });
            Assert.IsTrue(exception.Message.Contains("cannot define transition", StringComparison.Ordinal));
        }

        [Test]
        public void Is_Transition_Allowed_Test()
        {
            var allowedTransitions = new AllowedStateTransitions<ValidEnumOne>(
                ValidEnumOne.First,
                new[] { ValidEnumOne.Fifth, ValidEnumOne.Second });

            Assert.IsFalse(allowedTransitions.IsTransitionAllowed(ValidEnumOne.First));
            Assert.IsTrue(allowedTransitions.IsTransitionAllowed(ValidEnumOne.Second));
            Assert.IsFalse(allowedTransitions.IsTransitionAllowed(ValidEnumOne.Third));
            Assert.IsFalse(allowedTransitions.IsTransitionAllowed(ValidEnumOne.Fourth));
            Assert.IsTrue(allowedTransitions.IsTransitionAllowed(ValidEnumOne.Fifth));

            Assert.Throws<ArgumentException>(() =>
            {
                allowedTransitions.IsTransitionAllowed((ValidEnumOne)3);
            });
        }

        [Test]
        public void Should_Throw_On_Empty_Allowed_Transitions()
        {
            Assert.Throws<InvalidOperationException>(() =>
            {
                var allowedTransitions = new AllowedStateTransitions<ValidEnumOne>(
                    ValidEnumOne.Fifth,
                    Array.Empty<ValidEnumOne>());
                Console.WriteLine(allowedTransitions);
            });
        }
        
        [Test]
        public void Should_Throw_On_Duplicate_States()
        {
            var exception = Assert.Throws<ArgumentException>(() =>
            {
                var allowedTransitions = new AllowedStateTransitions<ValidEnumOne>(
                    ValidEnumOne.Fifth,
                    new[] { ValidEnumOne.First, ValidEnumOne.Second, ValidEnumOne.First });
                Console.WriteLine(allowedTransitions);
            });
            Assert.IsTrue(exception.Message.Contains("duplicated", StringComparison.Ordinal));
        }
    }
}
