using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;
using PluginSystem;

namespace PluginMorph
{
    public static class EbxUtils
    {
        public static Dictionary<string, string> ebxGuid;

        public static List<string> LoadEbxImport(EBX ebx)
        {
            var ImportValues = new List<string>();
            if (ebxGuid != null)
            {     
                foreach (EBX.FBGuid guid in ebx.imports.Keys)
                {
                    string key = guid.ToString().Replace("-", "");
                    if (ebxGuid.ContainsKey(key))
                    {
                        ImportValues.Add(ebxGuid[key]);
                    }
                    else
                    {
                        ImportValues.Add("Not found!");
                    }
                }
            }
            return ImportValues;
        }
    }
}
