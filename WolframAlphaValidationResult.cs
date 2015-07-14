using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WolframAlphaAPIv2
{
    public class WolframAlphaValidationResult
    {
        public bool Success { get; set; }
        public String ParseData { get; set; }
        public List<WolframAlphaAssumption> Assumptions { get; set; }
        public bool ErrorOccured { get; set; }
        public double Timing { get; set; }

    }
}
