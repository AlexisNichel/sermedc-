using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sermed.model
{
    class SensorClass
    {
        public String LedOn = "55AA24010200010000000000000000000000000000002701";
        public String LedOff = "55AA24010200000000000000000000000000000000002601";
        public String Verify = "55AA02010000000000000000000000000000000000000201";
        public String DeleteAll = "55AA06010000000000000000000000000000000000000601";
        public String SetTimeout = "55AA0E010200[value]000000000000000000000000000000";
    }
}
