using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PluginSystem
{
    public interface IMeshExporter
    {
        void ExportLod(MeshAsset mesh, int lodIndex, string targetfile, float scale = 1.0f);
        void ExportAllLods(MeshAsset mesh, string targetdir, float scale = 1.0f);

        void ExportLodWithMorph(MeshAsset mesh, int lodIndex, MorphStaticAsset morph, string targetfile, float scale = 1.0f, bool bake = false);

    }
}
