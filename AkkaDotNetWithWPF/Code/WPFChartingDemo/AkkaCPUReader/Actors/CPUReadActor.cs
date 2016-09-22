using Akka.Actor;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AkkaCPUReader
{
    /// <summary>
    /// The CPUReadActor is responsible for querying the Windows OS for CPU Data using a performance counter.
    /// The CPUReadActor waits for a ReadCPURequestMessage, then reads the data and sends it to the coordinator.
    /// 
    /// It uses an Akka.NET Scheduler to send a CPUReadRequestMessage to itself every 200ms.
    /// </summary>
    public class CPUReadActor : ReceiveActor
    {
        ICancelable _timer;
        PerformanceCounter _cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");

        public CPUReadActor()
        {
            Receive<ReadCPURequestMessage>(msg => ReceiveReadDataMessage());
        }

        private void ReceiveReadDataMessage()
        {
            float value = _cpuCounter.NextValue();
            Context.Parent.Tell(new DataMessage() { Value = value, Date = DateTime.Now });
        }

        protected override void PreStart()
        {
            _timer = Context.System
                .Scheduler
                .ScheduleTellRepeatedlyCancelable(
                    TimeSpan.FromSeconds(1),
                    TimeSpan.FromMilliseconds(200),
                    Self,
                    new ReadCPURequestMessage(),
                    Self);
        }
    }
}
