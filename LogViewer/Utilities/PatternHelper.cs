using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using LogViewer.Entities;

namespace LogViewer.Utilities
{
    public class PatternHelper
    {
        private static string patternPath;

        static PatternHelper()
        {
            patternPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Pattern.xml");
        }

        public static IList<Pattern> LoadPatterns()
        {
            var pats = new List<Pattern>();

            var doc = new XmlDocument();
            doc.Load(patternPath);
            var nodes = doc.SelectNodes("//pattern");

            foreach (XmlNode item in nodes)
            {
                var name = item.Attributes["name"].Value.ToString();
                var val = item.Attributes["value"].Value.ToString();

                if (!string.IsNullOrEmpty(name))
                {
                    pats.Add(new Pattern() { PatternName = name, PatternValue = val });
                }
            }
            return pats;
        }

        public static bool IsUnZip()
        {
            var doc = new XmlDocument();
            doc.Load(patternPath);

            var node = doc.SelectSingleNode("//settings/item[@key='IsUnZip']");

            if (node != null && node.Attributes["value"].Value.ToString().ToUpper() == "TRUE")
                return true;

            return false;
        }

        public static bool SavePattern(Pattern pattern)
        {
            var success = true;
            
            var doc = new XmlDocument();
            doc.Load(patternPath);

            var node = doc.SelectSingleNode(string.Format("//pattern[@name='{0}']", pattern.PatternName));

            var isPatternExist = node != null;

            if (isPatternExist)
            {
                node.Attributes["value"].Value = pattern.PatternValue;
            }
            else
            {
                var rootNode = doc.SelectSingleNode("/root");

                var newNode = doc.CreateElement("pattern");
                newNode.SetAttribute("name", pattern.PatternName);
                newNode.SetAttribute("value", pattern.PatternValue);

                rootNode.AppendChild(newNode);
            }

            doc.Save(patternPath);

            return success;
        }

        public static bool DeletePattern(Pattern pattern)
        {
            var success = true;

            var doc = new XmlDocument();
            doc.Load(patternPath);

            var node = doc.SelectSingleNode(string.Format("//pattern[@name='{0}']", pattern.PatternName));
            var rootNode = doc.SelectSingleNode("/root");
            var isPatternExist = node != null;

            if (isPatternExist)
            {
                rootNode.RemoveChild(node);
            }
            else
            {
                throw new Exception("Not pattern exist.");
            }

            doc.Save(patternPath);

            return success;
        }
    }
}
