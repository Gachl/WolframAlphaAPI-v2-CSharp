using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WolframAlphaAPIv2
{
    public class WolframAlphaPod
    {
        public List<WolframAlphaSubPod> SubPods { get; set; }
        public String Title { get; set; }
        public String Scanner { get; set; }
        public int Position { get; set; }
        public bool ErrorOccured { get; set; }
    }
}
