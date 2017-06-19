using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;

using PluginSystem;

namespace PluginMorph
{
    public class MorphStaticExtended : MorphStaticAsset
    {
        // ebx part
        private EBX ebx;
        public string PresetMesh;

        public MorphStaticExtended(MemoryStream resStream, string name) : base(resStream, name)
        {
            PresetMesh = "N/A";
        }
        
        public MorphStaticExtended(MemoryStream resStream, MemoryStream ebxStream, string name) : base (resStream, name) 
        {
            ebx = new EBX(ebxStream);
            var imports = EbxUtils.LoadEbxImport(ebx);
            if (imports.Count == 1)
            {
                PresetMesh = imports[0];
            }
        }
        
        public string ToJson()
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(this, Newtonsoft.Json.Formatting.Indented);
        }

        public void ExportToJson(string targetFile)
        {
            string json = Newtonsoft.Json.JsonConvert.SerializeObject(this, Newtonsoft.Json.Formatting.Indented);
            File.WriteAllText(targetFile, json);
        }
    }
}
