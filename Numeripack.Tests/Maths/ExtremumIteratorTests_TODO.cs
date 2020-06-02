using System;
using NUnit.Framework;

namespace Numeripack.Tests.Maths
{
    [TestFixture]
    internal class ExtremumIteratorTests
    {
        [Test]
        public void Quadratic_Equation_Test()
        {
            var iterator = new ExtremumIteratorBuilder()
                .Find(Extremum.Maximum)
                .ForObject(new QuadraticEquation(-4.2, 4.6, -12.3) { Argument = -23.3 })
                .SelectArgument(qe => qe.Argument)
                .WithStep(ctx =>
                {
                    var qe = ctx.Object;
                    var derivative = qe.GetDerivative();
                    qe.Argument += derivative * 0.01;
                    return qe.GetValue();
                })
                .WithMaximumSteps(1000)
                .Build();

            while (iterator.Next())
            {
                if (iterator.Current != null)
                {
                    Console.WriteLine($"{iterator.StepNumber}.  f({iterator.Current.Argument}) = {iterator.Current.Value}");
                }
            }

            // or

            //var result = iterator.FindResult();
        }
    }
}
