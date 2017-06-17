using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace VaderSharp
{
    public class ConfigStore
    {

        private static ConfigStore config;

        private Dictionary<string, double> boosterDict;
        public Dictionary<string, double> BoosterDict { get { return boosterDict; } }

        private string[] negations;
        public string[] Negations { get { return negations; } }

        private Dictionary<string, double> specialCaseIdioms;
        public Dictionary<string, double> SpecialCaseIdioms { get { return specialCaseIdioms; } }

        private ConfigStore(string languageCode)
        {
            LoadConfig(languageCode);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="languageCode">Language code in writing style "language-country". Default is British English.</param>
        /// <returns>ConfigStore object.</returns>
        public static ConfigStore CreateConfig(string languageCode = "en-gb")
        {
            config = config ?? new ConfigStore(languageCode);
            return config;
        }

        /// <summary>
        /// Initializes the ConfigStore and loads the config file.
        /// </summary>
        /// <param name="languageCode">Language code in writing style "language-country".</param>
        private void LoadConfig(string languageCode)
        {
            string path = $"D:/Daten/Repositories/vadersharp/VaderSharp/VaderSharp/strings/{languageCode}.xml";
            if (!File.Exists(path))
            {
                throw new FileNotFoundException("Language file was not found. Please check language code.");
            }
            XElement root = XDocument.Load(path).Document.Root;
            LoadNegations(root);
            LoadIdioms(root);
            LoadBooster(root);
        }

        /// <summary>
        /// Loads negations from config file.
        /// </summary>
        /// <param name="root">Root element of XML document</param>
        private void LoadNegations(XElement root)
        {
            var nodes = root.Descendants(XName.Get("negation"));
            int length = nodes.Count();
            negations = new string[length];
            for (int i = 0; i < length; i++)
            {
                negations[i] = nodes.ElementAt(i).Value;
            }
        }

        /// <summary>
        /// Loads idioms from config file.
        /// </summary>
        /// <param name="root">Root element of XML document</param>
        private void LoadIdioms(XElement root)
        {
            specialCaseIdioms = new Dictionary<string, double>();
            var nodes = root.Descendants(XName.Get("idiom"));
            double value;
            foreach (var n in nodes)
            {
                value = double.Parse(n.Attribute(XName.Get("value")).Value);
                specialCaseIdioms.Add(n.Value, value);
            }
        }

        /// <summary>
        /// Loads booster words from config file.
        /// </summary>
        /// <param name="root">Root element of XML document</param>
        private void LoadBooster(XElement root)
        {
            boosterDict = new Dictionary<string, double>();
            var nodes = root.Descendants(XName.Get("booster"));
            double sign;
            foreach (var n in nodes)
            {
                sign = n.Attribute(XName.Get("sign")).Value == "BIncr" ? 0.293 : -0.293;
                boosterDict.Add(n.Value, sign);
            }
        }
    }
}
