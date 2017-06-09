using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace PluginSystem
{
    public class MorphStaticAsset
    {
        // res part
        int Unknown1;
        public int LodCount;
        public int TotalSectionCount;  // count of sections across all lods (ie number of sections per lod = TotalSectionCount/LodCount)
        public int TotalVertexCount;
        public int BoneCount;
        long ChunkLength;
        long Offset1;
        long SectionChunkSizeOffset;
        public long VertexOffset;
        public long BonesOffset;

        private List<List<Vector>> VerticesByLodAndSections;

        public List<int> SectionVerticesOffsets;
        public List<Vector> BonesMorph;
        public List<Vector> Vertices;
        
        public MorphStaticAsset(MemoryStream s)
        {
            Unknown1 = Helpers.ReadInt(s);
            LodCount = Helpers.ReadInt(s);
            TotalSectionCount = Helpers.ReadInt(s);
            TotalVertexCount = Helpers.ReadInt(s);
            BoneCount = Helpers.ReadInt(s);
            ChunkLength = Helpers.ReadLong(s);
            Offset1 = Helpers.ReadLong(s);
            SectionChunkSizeOffset = Helpers.ReadLong(s);
            VertexOffset = Helpers.ReadLong(s);
            BonesOffset = Helpers.ReadLong(s);

            SectionVerticesOffsets = new List<int>();
            s.Seek(SectionChunkSizeOffset, SeekOrigin.Begin);
            for (int i=0; i < TotalSectionCount; i++)
            {
                SectionVerticesOffsets.Add(Helpers.ReadInt(s));
            }

            ReadVertices(s);
            ReadMorphBones(s);
        }

        private void ReadVertices(MemoryStream s)
        {
            Vertices = new List<Vector>();
            s.Seek(VertexOffset, SeekOrigin.Begin);
            for (int i = 0; i < TotalVertexCount; i++)
            {
                Vertices.Add(ReadVector(s));
            }
        }

        private void ReadMorphBones(MemoryStream s)
        {
            BonesMorph = new List<Vector>();
            s.Seek(BonesOffset, SeekOrigin.Begin);
            for (int i=0; i < BoneCount; i++)
            {
                BonesMorph.Add(ReadVector(s));
            }
        }

        private Vector ReadVector(MemoryStream s)
        {
            return new Vector(Helpers.ReadFloat(s), Helpers.ReadFloat(s), Helpers.ReadFloat(s), Helpers.ReadFloat(s));
        }

        public void ApplyMorphToMesh(MeshAsset mesh)
        {
            for (int i = 0; i < LodCount; i++)
            {
                ApplyMorphToMeshLod(i, mesh);
            }
        }

        private void ApplyMorphToMeshLod(int lodIndex, MeshAsset mesh)
        {
            List<Vector> LodVertices = GetVerticesForLod(lodIndex, mesh);
            MeshLOD lod = mesh.lods[lodIndex];
            int offset = 0;
            foreach (MeshLodSection section in lod.sections)
            {
                if (section.vertices != null)
                {
                    for (int i = 0; i < section.vertices.Count; i++)
                    {
                        section.vertices[i].position.members[0] = LodVertices[offset].members[0];
                        section.vertices[i].position.members[1] = LodVertices[offset].members[1];
                        section.vertices[i].position.members[2] = LodVertices[offset].members[2];        
                        offset++;
                    }
                }
                else
                {
                    // if the lod is not completely loaded, we abort
                    break;
                }
            }
        }

        public void ApplyMorphToSkeleton(SkeletonAsset skeleton)
        {
            for (int i = 0; i < skeleton.Bones.Count; i++)
            {
                FBBone fbbone = skeleton.Bones[i];
                Vector boneOffset = BonesMorph[i];
                //var tmp = fbbone.Location.members;
                fbbone.Location.members[0] += boneOffset.members[0];
                fbbone.Location.members[1] += boneOffset.members[1];
                fbbone.Location.members[2] += boneOffset.members[2];
                //fbbone.Location.members = tmp;
            }
        }

        public void Apply(MeshAsset mesh, SkeletonAsset skeleton)
        {
            if (mesh != null)
            {
                ApplyMorphToMesh(mesh);
            }
            if (skeleton != null)
            {
                ApplyMorphToSkeleton(skeleton);
            }
        }

        public List<Vector> GetVerticesForLod(int lodIndex, MeshAsset presetMesh)
        {
            int startOffset = 0;
            for (int i = 0; i < lodIndex; i++)
            {
                startOffset = startOffset + presetMesh.lods[i].GetLODTotalVertCount();
            }

            var VerticesForLod = new List<Vector>();
            VerticesForLod.AddRange(Vertices.GetRange(startOffset, presetMesh.lods[lodIndex].GetLODTotalVertCount()));
            return VerticesForLod;
        }

        private bool CheckVectorNotZero(Vector v)
        {
            return v.members[0] != 0.0 || v.members[1] != 0.0 || v.members[2] != 0.0;
        }
    }
}
