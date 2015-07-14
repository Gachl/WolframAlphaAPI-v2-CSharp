# WolframAlphaAPI-v2-CSharp
Wolfram Alpha C# Interface for API v2
As it may be provided by http://products.wolframalpha.com/api/libraries.html

This is simply a rewritten version of the .NET API Wolfram Alpha offers. However their version is based on VisualBasic and uses their API v1 which is not in use anymore and therefore the whole library doesn't work. I've fixed most issues, there are still two or three lines of code that make absolutely no sense but I left them in just in case.

Feel free to contribute. Please resubmit to Wolfram Alpha if you do any useful changes.

# Example code
```
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
```
