using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace WolframAlphaAPIv2
{
    public enum WolframAlphaQueryFormat
    {
        /// <summary>
        /// Image
        /// </summary>
        image,

        /// <summary>
        /// HTML
        /// </summary>
        html,

        /// <summary>
        /// PDF
        /// </summary>
        pdf,

        /// <summary>
        /// Plain text
        /// </summary>
        plaintext,

        /// <summary>
        /// Mathematica Input
        /// </summary>
        minput,

        /// <summary>
        /// Mathematica Output
        /// </summary>
        moutput,

        /// <summary>
        /// Mathematica Math Markup Language
        /// </summary>
        mathml,

        /// <summary>
        /// Mathematica Expression Markup Language
        /// </summary>
        expressionml,

        /// <summary>
        /// XML
        /// </summary>
        xml
    }

    public class WolframAlphaQuery
    {
        public String APIKey { get; set; }

        public bool MoreOutput { get; set; }
        public WolframAlphaQueryFormat Format { get; set; }
        public bool Asynchronous { get; set; }
        public bool AllowCaching { get; set; }
        public String Query { get; set; }
        public int TimeLimit { get; set; }

        private string substitution;
        private string assumption;
        private string podTitle;

        public String[] Substitutions
        {
            get
            {
                return this.substitution.Split(new[] { "&substitution=" }, StringSplitOptions.RemoveEmptyEntries);
            }
        }

        public String[] Assumptions
        {
            get
            {
                return this.assumption.Split(new[] { "&assumption=" }, StringSplitOptions.RemoveEmptyEntries);
            }
        }

        public String[] PodTitles
        {
            get
            {
                return this.podTitle.Split(new[] { "&assumption=" }, StringSplitOptions.RemoveEmptyEntries);
            }
        }

        public String FullQueryString
        {
            get
            {
                return String.Format("?appid={0}&moreoutput={1}&timelimit={2}&format={3}&input={4}{5}{6}", this.APIKey, this.MoreOutput, this.TimeLimit, this.Format, this.Query, this.assumption, this.substitution);
            }
        }

        public void AddPodTitle(string podTitle, bool checkForDuplicates = false)
        {
            string fullPodTitle = String.Format("&podtitle={0}", HttpUtility.UrlEncode(podTitle));
            if (checkForDuplicates && this.podTitle.Contains(fullPodTitle))
                return;
            this.podTitle += fullPodTitle;
        }

        public void AddSubstitution(string substitution, bool checkForDuplicates = false)
        {
            string fullSubstitution = String.Format("&substitution={0}", HttpUtility.UrlEncode(substitution));
            if (checkForDuplicates && this.substitution.Contains(fullSubstitution))
                return;
            this.substitution += fullSubstitution;
        }

        public void AddAssumption(string assumption, bool checkForDuplicates = false)
        {
            string fullAssumption = String.Format("&assumption={0}", HttpUtility.UrlEncode(assumption));
            if (checkForDuplicates && this.assumption.Contains(fullAssumption))
                return;
            this.substitution += fullAssumption;
        }

        public void AddAssumption(WolframAlphaAssumption assumption, bool checkForDuplicates = false)
        {
            this.AddAssumption(assumption.Word, checkForDuplicates);
        }
    }
}
