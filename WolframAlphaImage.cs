using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WolframAlphaAPIv2
{
    public class WolframAlphaImage
    {
        public Uri Location { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public String Title { get; set; }
        public String HoverText { get; set; }
    }
}
