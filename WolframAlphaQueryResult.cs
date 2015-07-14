using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WolframAlphaAPIv2
{
    public class WolframAlphaQueryResult
    {
        public List<WolframAlphaPod> Pods { get; set; }
        public bool Success { get; set; }
        public bool ErrorOccured { get; set; }
        public string DataTypes { get; set; }
        public string TimedOut { get; set; }
        public double Timing { get; set; }
        public double ParseTiming { get; set; }
    }
}
