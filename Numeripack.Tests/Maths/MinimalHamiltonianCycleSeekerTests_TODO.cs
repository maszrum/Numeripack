using NUnit.Framework;

namespace Numeripack.Tests.Maths
{
    [TestFixture]
    internal class MinimalHamiltonianCycleSeekerTests
    {
        //[Test]
        //public static void Extreme_Hamiltonian_Path_Seeker()
        //{
        //    var epp = new ElevatorPhysicalParameters()
        //    {
        //        AccelerationTime = 0.5,
        //        StepSizeMilliseconds = 50,
        //        Velocity = 6,
        //        WaitingAverageTime = 5
        //    };

        //    var state = new PhysicalState()
        //    {
        //        Position = 10.4,
        //        Velocity = -3.4
        //    };

        //    var seeker = new MinimalHamiltonianCycleSeekerBuilder<double>()
        //        .ForElements(new[] { -3.4, 12.5, 4.5, -55.3, 0.23 })
        //        .WithCostFunction((from, to, index) =>
        //        {
        //            if (index == 0)
        //            {
        //                var movementBuilder = new ElevatorMovementBuilder(epp, state);
        //                try
        //                {
        //                    movementBuilder.AddMovementTo(to);
        //                }
        //                catch (InvalidOperationException)
        //                {
        //                    return 99999;
        //                }

        //                var time = movementBuilder.Get().GetFinalState().Time;
        //                return time;
        //            }
        //            else
        //            {
        //                var fromState = new PhysicalState() { Position = from };
        //                var movementBuilder = new ElevatorMovementBuilder(epp, fromState);
        //                movementBuilder.AddMovementTo(to);

        //                var time = movementBuilder.Get().GetFinalState().Time + epp.WaitingAverageTime;
        //                return time;
        //            }
        //        })
        //        .Build();

        //    while (seeker.Next())
        //    {
        //        if (seeker.Current != null)
        //        {
        //            Console.WriteLine(
        //                $"{seeker.StepNumber}.  f({state.Position} -> {seeker.Current.Argument}) = {seeker.Current.Value}");
        //        }
        //    }

        //    // or

        //    //var result = seeker.FindResult();

        //    //Console.WriteLine($"{seeker.StepNumber}.  f({result.Argument}) = {result.Value}");
        //}
    }
}
