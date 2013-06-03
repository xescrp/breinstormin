using System.Collections.Generic;

namespace breinstormin.tools.visualstudio
{

    public class VSProjectConfiguration
    {
        public string Condition
        {
            get { return condition; }
            set { condition = value; }
        }

        public IDictionary<string, string> Properties
        {
            get { return properties; }
        }

        private readonly Dictionary<string, string> properties = new Dictionary<string, string>();
        private string condition;
    }
}