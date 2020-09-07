using PDF_Extractor_API.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PDF_Extractor_API.Utils
{
    class Mapper
    {
        public const int DEFAULT_KEY = 0;
        public const int SNAKE_CASE = 1;
        public const int CAPS = 2;

        public static string MatchAndMap(string text, string pattern)
        {
            return Regex.Match(text, pattern).Groups[1].Value;
        }

        public static Dictionary<string, string> MatchAndMap(string text, List<List<string>> keyPattern, int keyMap = DEFAULT_KEY)
        {
            Dictionary<string, string> results = new Dictionary<string, string>();
            foreach(List<string> pattern in keyPattern)
            {
                string match = Mapper.MatchAndMap(text, pattern[1]);
                results.Add(mapKey(pattern[0], keyMap), match);
            }

            return results;
        }

        public static TableMap MapTable(string text, string startWith, string endWith, List<List<string>> keyPattern)
        {
            TableMap tableMap = new TableMap();
            tableMap.Headers = (from k in keyPattern select k[0]).ToList();
            string patterns = string.Join(@"\s+", (from k in keyPattern select k[1]).ToArray());
            string pointer = String.Join(@"\s", tableMap.Headers.ToArray()) + @"\s+";
            string table = MatchAndMap(text, startWith+@"([\w\W\s]+)"+endWith);
            //table = MatchAndMap(table, pointer + @"([\w\W\s]+)");            
            table = Regex.Replace(table, pointer, "");
            table = Regex.Replace(table, @"\d+ \/ \d+", "");
            Console.WriteLine(table);
            MatchCollection result = Regex.Matches(table, patterns, RegexOptions.Multiline);
            foreach(Match match in result)
            {
                List<string> row = new List<string>();
                for (int i = 1; i <= match.Groups.Count; i++)
                {
                    string col = match.Groups[i].Value;
                    col = Regex.Replace(col, @"\s", " ");
                    row.Add(col);
                }
                tableMap.Rows.Add(row);
            }
            return tableMap;
        }

        private static string mapKey(string key, int keyMap)
        {
            string formattedKey = key;
            switch (keyMap)
            {
                case SNAKE_CASE:
                    formattedKey = Regex.Replace(formattedKey.Trim(), @"([\s\W]+)", "_").ToLower().TrimEnd('_');
                    break;
                case CAPS:
                    formattedKey = Regex.Replace(formattedKey.Trim(), @"(\w+)", delegate (Match match) {
                        string matchKey = match.Groups[1].Value;
                        return matchKey.ToUpper();
                    });
                    break;
                default:
                    formattedKey = key;
                    break;
            }
            return formattedKey;
        }

        public static void SaveMapping(List<List<string>> mappingCollection, string path)
        {
            using (Stream stream = File.Open(path, FileMode.Create)) {
                var bSerializer = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                bSerializer.Serialize(stream, mappingCollection);
            }
        }

        public static List<List<string>> LoadMapping(string path)
        {
            using (Stream stream = File.Open(path, FileMode.Open))
            {
                var bSerializer = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                return (List<List<string>>)bSerializer.Deserialize(stream);
            }
        }
    }
}
