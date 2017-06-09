using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;

namespace PluginMorph
{
    class PluginMorphCache
    {
        private static string MORPH_CACHE_FILE = "morphcache.txt";
        
        public static bool CheckForCache()
        {
            return File.Exists(MORPH_CACHE_FILE);
        }

        public static Dictionary<string, List<string>> LoadFromCache()
        {
            if (CheckForCache())
            {
                string cache = File.ReadAllText(MORPH_CACHE_FILE);
                return Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, List<string>>>(cache);
            }
            return null;
            
        }

        public static void WriteCache(Dictionary<string, List<string>> bundles)
        {
            if (File.Exists(MORPH_CACHE_FILE))
            {
                File.Delete(MORPH_CACHE_FILE);
            }
            string cache = Newtonsoft.Json.JsonConvert.SerializeObject(bundles);
            File.WriteAllText(MORPH_CACHE_FILE, cache);
        }

        public static void CleanCache()
        {
            if (File.Exists(MORPH_CACHE_FILE))
            {
                File.Delete(MORPH_CACHE_FILE);
            }
        }
    }
}
