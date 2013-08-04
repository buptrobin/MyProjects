using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ParseAdsUserClickConfig
{
    using System.IO;

    class Program
    {
        static void Main(string[] args)
        {
            var properties = ReadPropertiesFile("");
            
            var adcFeatures = new Dictionary<string, Dictionary<string, string>>();

            var assembly = System.Reflection.Assembly.GetExecutingAssembly();
            //Console.WriteLine(assembly.Location.);

            foreach (var secName in properties.Keys)
            {
                if (secName.Equals("GlobalConfig") || secName.Equals("FeatureSignalKeyConfig")) continue;
                string featurename = secName;
                var countMap = new Dictionary<string, string>();
                var featureMap = properties[secName];
                foreach (var idname in featureMap.Keys)
                {
            string path = @"d:\tmp\AdsUserClick_ServiceConfig.ini";
                    if(idname.StartsWith("QlfId_"))
                    {
                        countMap.Add(featureMap[idname], idname.Replace("QlfId_",""));                        
                    }
                }
                adcFeatures.Add(secName, countMap);
            }

            foreach (var adcFeature in adcFeatures.Keys)
            {
                Console.WriteLine(adcFeature);
                foreach (var kvp in adcFeatures[adcFeature])
                {
                    Console.Write(kvp.Key+":"+kvp.Value+"\t");
                }
                Console.WriteLine();
            }
            Console.ReadKey();

        }



        /// <summary>
        /// Parse the properties file.
        /// example of property file:
        /// [NeuralNet]
        /// Inputs=85
        /// Layers=2
        /// NumAdditionalQuery=1
        ///   
        /// [Input:1]
        /// ;OldName=Feature_1_garbage1_none
        /// Expression=(if (> QueryLevelFeature_626 0) 1 QueryLevelFeature_626)
        /// Intercept=-0.421485
        /// Slope=2.79405
        /// Transform=Freeform
        /// </summary>
        /// <param name="path">The file path of the properties file</param>
        /// <returns>Dictionary per section name</returns>
        public static Dictionary<string, Dictionary<string, string>> ReadPropertiesFile(string filepath)
        {
            if (string.IsNullOrEmpty(filepath))
            {
                return null;
            }

            if (!File.Exists(filepath))
            {
                throw new Exception("Not found file " + filepath);
            }

            var ret = new Dictionary<string, Dictionary<string, string>>();
            string[] ss = File.ReadAllLines(filepath);

            if (ss.Length < 1)
            {
                throw new Exception(string.Format("The file {0} is empty.", filepath));
            }

            bool flag = false;
            string sectionName = string.Empty;
            var section = new Dictionary<string, string>();
            foreach (var s0 in ss)
            {
                string s = s0.Trim();
                if (string.IsNullOrEmpty(s) || s.StartsWith(";"))
                {
                    continue;
                }

                if (s.StartsWith("["))
                {
                    if (flag)
                    {
                        ret.Add(sectionName, section);
                    }

                    section = new Dictionary<string, string>();
                    sectionName = s.Substring(1, s.Length - 2);
                    flag = true;
                }
                else
                {
                    if (flag)
                    {
                        int p = s.IndexOf('=');
                        string key = s.Substring(0, p);
                        string value = s.Substring(p + 1);
                        section.Add(key, value);
                    }
                }
            }

            return ret;
        }
    }
}
