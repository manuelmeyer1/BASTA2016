using Akka.Actor;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AkkaCPUReader
{
    /// <summary>
    /// The ChartingActor is responsible for talking to the User Interface.
    /// The current solution is simplified and just uses a delegate that is provided by the
    /// WPF project and sets properties on the viewmodel.
    /// 
    /// The call to the delegate is dispatched to the WPF UI thread by the WPF
    /// DataBinding Engine. (Only works with .NET 4.5 and higher).
    /// </summary>
    public class ChartingActor : ReceiveActor
    {
        private Action<float, DateTime> _dataPointSetter;

        public ChartingActor(Action<float, DateTime> dataPointSetter)
        {
            this._dataPointSetter = dataPointSetter;

            Receive<DrawPointMessage>(msg => ReceiveDrawPointMessage(msg));
        }

        private void ReceiveDrawPointMessage(DrawPointMessage msg)
        {
            _dataPointSetter(msg.Value, msg.Date);
        }
    }
}
