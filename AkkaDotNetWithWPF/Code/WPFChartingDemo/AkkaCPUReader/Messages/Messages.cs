using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AkkaCPUReader
{
    class ReadCPURequestMessage { }

    class DataMessage
    {
        public float Value { get; set; }
        public DateTime Date { get; set; }
    }

    class DrawPointMessage
    {
        public float Value { get; set; }
        public DateTime Date { get; set; }
    }
}
