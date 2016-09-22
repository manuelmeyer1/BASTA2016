using Akka.Actor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AkkaCPUReader
{

    /// <summary>
    /// The ActorSystem is the entrance Point used by the WPF Project to start the ActorSystem.
    /// It takes a delegate as a parameter that is used to update data in the UI.
    /// </summary>
    public class ActorSystemReference
    {
        private static ActorSystem _actorSystem;

        public static void CreateActorSystem(Action<float, DateTime> SetDataPointAction)
        {
            _actorSystem = ActorSystem.Create("akka");

            Action<float, DateTime> DataPointSetter = new Action<float, DateTime>((v, d) => SetDataPointAction(v, d));
            var chartingActor = _actorSystem.ActorOf(Props.Create(() => new ChartingActor(DataPointSetter)));

            var coordActor = _actorSystem.ActorOf(Props.Create(() => new CoordinatorActor(chartingActor)), "Coord");
        }
    }
}
