using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WolframAlphaAPIv2
{
    class Example
    {
        public static void Main(string[] args) // or something like this
        {
            WolframAlphaEngine engine = new WolframAlphaEngine("your-key-here");
            WolframAlphaQuery query = new WolframAlphaQuery()
            {
                Query = "1500kWh in Joules",
                Format = WolframAlphaQueryFormat.plaintext
            };

            WolframAlphaValidationResult validation = engine.ValidateQuery(query);
            if (validation.ErrorOccured)
            {
                Console.WriteLine("Couldn't comprehend: {0}", validation.ParseData);
                return;
            }
            WolframAlphaQueryResult result = engine.LoadResponse(query);
            WolframAlphaSubPod subPod = result.Pods.Where(x => x.Title == "Result").FirstOrDefault().SubPods.FirstOrDefault();
            if (subPod != null)
                Console.WriteLine(subPod.PodText);
        }
    }
}
