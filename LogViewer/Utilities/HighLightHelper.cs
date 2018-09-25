using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using LogViewer.Entities;

namespace LogViewer.Utilities
{
    public static class HighLightHelper
    {
        private static string highLightPath;

        static HighLightHelper()
        {
            highLightPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Pattern.xml");
        }

        public static IList<HighLight> LoadHighLights()
        {
            IList<HighLight> pats = new List<HighLight>();

            var doc = new XmlDocument();
            doc.Load(highLightPath);
            var nodes = doc.SelectNodes("//highLight");

            foreach (XmlNode item in nodes)
            {
                var name = item.Attributes["name"].Value.ToString();
                var val = item.Attributes["value"].Value.ToString();

                if (!string.IsNullOrEmpty(name))
                {
                    pats.Add(new HighLight() { HighLightName = name, HighLightValue = val });
                }
            }
            return pats;
        }

        public static bool SaveHighLight(HighLight highLight)
        {
            var success = true;
            
            var doc = new XmlDocument();
            doc.Load(highLightPath);

            var node = doc.SelectSingleNode(string.Format("//highLight[@name='{0}']", highLight.HighLightName));

            var isHighLightExist = node != null;

            if (isHighLightExist)
            {
                node.Attributes["value"].Value = highLight.HighLightValue;
            }
            else
            {
                var rootNode = doc.SelectSingleNode("/root");

                var newNode = doc.CreateElement("highLight");
                newNode.SetAttribute("name", highLight.HighLightName);
                newNode.SetAttribute("value", highLight.HighLightValue);

                rootNode.AppendChild(newNode);
            }

            doc.Save(highLightPath);

            return success;
        }

        public static bool DeleteHighLight(HighLight highLight)
        {
            var success = true;

            var doc = new XmlDocument();
            doc.Load(highLightPath);

            var node = doc.SelectSingleNode(string.Format("//highLight[@name='{0}']", highLight.HighLightName));
            var rootNode = doc.SelectSingleNode("/root");
            var isHighLightExist = node != null;

            if (isHighLightExist)
            {
                rootNode.RemoveChild(node);
            }
            else
            {
                throw new Exception("Not highLight exist.");
            }

            doc.Save(highLightPath);

            return success;
        }
    }
}
