using Akka.Actor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AkkaCPUReader
{
    /// <summary>
    /// The CordinatorActor keeps track of the dataCollector child actors and waits for them
    /// to send data.
    /// The data is then forwarded to the chartingActor.
    /// At the moment, the child actors are hardcoded inside the CoordinatorActor
    /// </summary>
    public class CoordinatorActor : ReceiveActor
    {
        List<IActorRef> _collectorActors;
        IActorRef _chartingActor;

        public CoordinatorActor(IActorRef chartingActor)
        {
            _collectorActors = new List<IActorRef>();
            _chartingActor = chartingActor;

            IActorRef cpuReadActor = Context.ActorOf(Props.Create(() => new CPUReadActor()),
                "CPUReadActor");
            _collectorActors.Add(cpuReadActor);


            Receive<DataMessage>(msg => ReceiveDataMessage(msg));
        }


        private void ReceiveDataMessage(DataMessage msg)
        {
            _chartingActor.Tell(new DrawPointMessage() { Value = msg.Value, Date = msg.Date });
        }
    }
}
