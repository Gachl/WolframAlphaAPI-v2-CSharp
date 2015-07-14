using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Xml;

namespace WolframAlphaAPIv2
{
    public class WolframAlphaEngine
    {
        private static readonly string baseUrl = "http://api.wolframalpha.com/v2/";
        private static readonly string baseUrlValidation = "validatequery";
        private static readonly string baseUrlQuery = "query";

        public WolframAlphaEngine(String apikey)
        {
            this.APIKey = apikey;
        }

        public String APIKey { get; set; }
        public WolframAlphaQueryResult QueryResult { get; private set; }
        public WolframAlphaValidationResult ValidationResult { get; private set; }

        /// <summary>
        /// Validate a query to check for issues before execution.
        /// </summary>
        /// <param name="query">The query to check for errors</param>
        /// <returns>A validation result</returns>
        public WolframAlphaValidationResult ValidateQuery(WolframAlphaQuery query)
        {
            if (String.IsNullOrEmpty(query.APIKey))
            {
                if (String.IsNullOrEmpty(this.APIKey))
                    throw new Exception("To use the Wolfram Alpha API v2, you must specify an API key either through the WolframAlphaQuery, or on the WolframAlphaEngine itself.");
                query.APIKey = this.APIKey;
            }

            if (query.Asynchronous && query.Format == WolframAlphaQueryFormat.html)
                throw new Exception("Wolfram Alpha does not allow asynchronous operations while the format for the query is not set to \"HTML\".");
            // Sorry, I took most of this code from the previous .NET interface written in VB and there are SO MANY ERRORS.
            // I tried to fix most of them but this one is a special one.
            // The exceptions says async is not allowed while format is not set to html, that means only html can do async,
            // however the code in the if parameter shows exactly the opposite.
            // I cba to figure out what was meant so I'll implement this 1:1. If you ever figure it out, fix it and submit it. Thank you.

            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(String.Format("{0}{1}{2}", WolframAlphaEngine.baseUrl, WolframAlphaEngine.baseUrlValidation, query.FullQueryString));
            webRequest.KeepAlive = true;
            string response = new StreamReader(webRequest.GetResponse().GetResponseStream()).ReadToEnd();

            return this.ValidateQuery(response);
        }

        public WolframAlphaValidationResult ValidateQuery(string response)
        {
            XmlDocument document = new XmlDocument();
            WolframAlphaValidationResult result = null;
            try
            {
                document.LoadXml(response);
                result = ValidateQuery(document);
            }
            catch
            {
                return new WolframAlphaValidationResult() { ErrorOccured = true }; // We can't help with misformatted XML or failure to build the result from it.
            }

            return result;
        }

        public WolframAlphaValidationResult ValidateQuery(XmlDocument document)
        {
            XmlNode mainNode = document.SelectNodes("/validatequeryresult")[0];

            WolframAlphaValidationResult result = new WolframAlphaValidationResult()
            {
                Success = bool.Parse(mainNode.Attributes["success"].Value),
                ErrorOccured = bool.Parse(mainNode.Attributes["error"].Value),
                Timing = double.Parse(mainNode.Attributes["timing"].Value),
                Assumptions = new List<WolframAlphaAssumption>()
            };

            if (mainNode.SelectNodes("parsedata").Count > 0)
                result.ParseData = mainNode.SelectNodes("parsedata")[0].InnerText;

            foreach (XmlNode node in mainNode.SelectNodes("assumptions"))
            {
                WolframAlphaAssumption assumption = new WolframAlphaAssumption();

                if (node.SelectNodes("word").Count > 0)
                    assumption.Word = node.SelectNodes("word")[0].InnerText;

                if (node.SelectNodes("categories").Count > 0)
                    foreach (XmlNode contentNode in node.SelectNodes("categories")[0].SelectNodes("category"))
                        assumption.Categories.Add(contentNode.InnerText);

                result.Assumptions.Add(assumption);
            }

            return result;
        }

        /// <summary>
        /// Run a query.
        /// </summary>
        /// <param name="query">The query to run</param>
        /// <returns>A query result</returns>
        public WolframAlphaQueryResult LoadResponse(WolframAlphaQuery query)
        {
            if (String.IsNullOrEmpty(query.APIKey))
            {
                if (String.IsNullOrEmpty(this.APIKey))
                    throw new Exception("To use the Wolfram Alpha API v2, you must specify an API key either through the WolframAlphaQuery, or on the WolframAlphaEngine itself.");
                query.APIKey = this.APIKey;
            }

            if (query.Asynchronous && query.Format == WolframAlphaQueryFormat.html)
                throw new Exception("Wolfram Alpha does not allow asynchronous operations while the format for the query is not set to \"HTML\".");
            // Please see ValidateQuery(WolframAlphaQuery query) above

            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(String.Format("{0}{1}{2}", WolframAlphaEngine.baseUrl, WolframAlphaEngine.baseUrlQuery, query.FullQueryString));
            webRequest.KeepAlive = true;
            string response = new StreamReader(webRequest.GetResponse().GetResponseStream()).ReadToEnd();

            return this.LoadResponse(response);
        }

        public WolframAlphaQueryResult LoadResponse(string response)
        {
            XmlDocument document = new XmlDocument();
            WolframAlphaQueryResult result = null;
            try
            {
                document.LoadXml(response);
                result = LoadResponse(document);
            }
            catch
            {
                return new WolframAlphaQueryResult() { Success = false }; // We can't help with misformatted XML or failure to build the result from it.
            }

            return result;
        }

        public WolframAlphaQueryResult LoadResponse(XmlDocument document)
        {
            XmlNode mainNode = document.SelectNodes("/queryresult")[0];

            WolframAlphaQueryResult result = new WolframAlphaQueryResult()
            {
                Success = bool.Parse(mainNode.Attributes["success"].Value),
                ErrorOccured = bool.Parse(mainNode.Attributes["error"].Value),
                Timing = double.Parse(mainNode.Attributes["timing"].Value),
                TimedOut = mainNode.Attributes["timedout"].Value,
                DataTypes = mainNode.Attributes["datatypes"].Value,
                Pods = new List<WolframAlphaPod>()
            };

            foreach (XmlNode node in mainNode.SelectNodes("pod"))
            {
                WolframAlphaPod pod = new WolframAlphaPod()
                {
                    Title = node.Attributes["title"].Value,
                    Scanner = node.Attributes["scanner"].Value,
                    Position = int.Parse(node.Attributes["position"].Value),
                    ErrorOccured = bool.Parse(node.Attributes["error"].Value),
                    SubPods = new List<WolframAlphaSubPod>()
                };

                foreach (XmlNode subNode in node.SelectNodes("subpod"))
                {
                    WolframAlphaSubPod subPod = new WolframAlphaSubPod()
                    {
                        Title = subNode.Attributes["title"].Value
                    };

                    foreach (XmlNode contentNode in subNode.SelectNodes("plaintext"))
                        subPod.PodText = contentNode.InnerText; // I have no idea why this is assigned over and over again until the last but I'll just roll with it.

                    foreach (XmlNode contentNode in subNode.SelectNodes("img"))
                        subPod.PodImage = new WolframAlphaImage()
                        {
                            Location = new Uri(contentNode.Attributes["src"].Value),
                            HoverText = contentNode.Attributes["alt"].Value,
                            Title = contentNode.Attributes["title"].Value,
                            Width = int.Parse(contentNode.Attributes["width"].Value),
                            Height = int.Parse(contentNode.Attributes["height"].Value)
                        };

                    pod.SubPods.Add(subPod);
                }

                result.Pods.Add(pod);
            }

            return result;
        }
    }
}
