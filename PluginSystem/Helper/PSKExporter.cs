using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PluginSystem;

namespace PluginSystem
{
    public class PSKExporter : ASkinnedMeshExporter, IMeshExporter
    {
        private bool Reverse = true;

        public PSKExporter(SkeletonAsset skel)
            : base(skel)
        {

        }

        public void ExportLod(MeshAsset mesh, int lodIndex, string targetFile, float scale = 1.0f)
        {
            byte[] pskData = ExportSkinnedMeshToPsk(Skeleton, mesh.lods[0], scale);
            File.WriteAllBytes(targetFile, pskData);
        }
        public void ExportAllLods(MeshAsset mesh, string targetdir, float scale = 1.0f)
        {
            foreach (MeshLOD lod in mesh.lods)
            {
                string targetFile = Path.Combine(targetdir, lod.shortName + ".psk");
                byte[] data = ExportSkinnedMeshToPsk(Skeleton, lod, scale);
                File.WriteAllBytes(targetFile, data);
            }
        }

        public void ExportLodWithMorph(MeshAsset mesh, int lodIndex, MorphStaticAsset morph, string targetfile, float scale = 1.0f, bool bake = false)
        {
            morph.ApplyMorphToMesh(mesh);
            ExportLod(mesh, lodIndex, targetfile, scale);

            // export psa file holding a pose corresponding to the skeleton morph
            string psaFileName = Path.GetFileNameWithoutExtension(targetfile) + ".psa";
            string targetPsaFile = Path.Combine(Path.GetDirectoryName(targetfile), psaFileName);
            try
            {
                byte[] psaData = ExportPoseToPsa(Skeleton, morph.BonesMorph, scale);
                File.WriteAllBytes(targetPsaFile, psaData);
            }
            catch (ArgumentNullException)
            {
                Console.WriteLine("Unable to export pose as psa : skeleton not provided");
            }  
        }

        // export given LOD to psk
        private byte[] ExportSkinnedMeshToPsk(SkeletonAsset skeleton, MeshLOD LOD, float OverrideScale = 1.0f)
        {
            PSKFile Psk = new PSKFile();
            Psk.points = new List<PSKFile.PSKPoint>();
            Psk.edges = new List<PSKFile.PSKEdge>();
            Psk.materials = new List<PSKFile.PSKMaterial>();
            Psk.bones = new List<PSKFile.PSKBone>();
            Psk.faces = new List<PSKFile.PSKFace>();
            Psk.weights = new List<PSKFile.PSKWeight>();

            // create skeleton (only if mesh is skinned mesh)
            if (LOD.type == MeshType.MeshType_Skinned)
                Psk.bones = CreatePskSkeleton(skeleton, OverrideScale);
            
            // create mesh
            int offset = 0;
            int matIdx = 0;
            for (int bufIdx = 0; bufIdx < LOD.sections.Count; bufIdx++)
            {
                MeshLodSection MeshBuffer = LOD.sections[bufIdx];
                for (int i = 0; i < MeshBuffer.vertices.Count; i++)
                {
                    if (MeshBuffer.vertices[i].position.members.Length != 3) MeshBuffer.vertices[i].position.members = new float[3];
                    if (MeshBuffer.vertices[i].texCoords.members.Length != 2) MeshBuffer.vertices[i].texCoords.members = new float[2];
                    Vector p = new Vector(MeshBuffer.vertices[i].position.members[0] * OverrideScale, MeshBuffer.vertices[i].position.members[1] * OverrideScale, MeshBuffer.vertices[i].position.members[2] * OverrideScale);
                    Psk.points.Add(new PSKFile.PSKPoint(ConvertVector3ToPsk(p)));
                    Vector tc = new Vector(MeshBuffer.vertices[i].texCoords.members[0], MeshBuffer.vertices[i].texCoords.members[1]);
                    Psk.edges.Add(new PSKFile.PSKEdge((ushort)(offset + i), ConvertVector2ToPsk(tc), (byte)matIdx));
                    if(MeshBuffer.vertices[i].boneWeights != null)
                        for (int x = 0; x < 4; x++)
                        {
                            float Weight = MeshBuffer.vertices[i].boneWeights[x];

                            // only add meaningful weights
                            if (Weight != 0.0f)
                            {
                                int BoneIndex = MeshBuffer.vertices[i].boneIndices[x];

                                int SubObjectBoneIndex = MeshBuffer.boneIndices[BoneIndex];
                                Psk.weights.Add(new PSKFile.PSKWeight(
                                    Weight,
                                    (int)(offset + i),
                                    SubObjectBoneIndex
                                    ));
                            }
                        }
                }

                // reverse indices order before building faces: necessary for correct normal building since all points have been mirrored along z-axis.
                // if this is not done, all normals are flipped inside.
                if (Reverse) MeshBuffer.indicies.Reverse();
                for (int fi = 0; fi < MeshBuffer.indicies.Count; fi++)
                {
                    if (fi % 3 == 0)
                    {
                        Psk.faces.Add(new PSKFile.PSKFace(
                            (int)(offset + MeshBuffer.indicies[fi]),
                            (int)(offset + MeshBuffer.indicies[fi + 1]),
                            (int)(offset + MeshBuffer.indicies[fi + 2]),
                            (byte)matIdx)
                        );
                    }
                }
                Psk.materials.Add(new PSKFile.PSKMaterial(LOD.sections[bufIdx].matName, matIdx));

                offset += (int)LOD.sections[bufIdx].vertCount;
                matIdx++;
            }
            return Psk.SaveToMemory().ToArray();
        }

        private byte[] ExportPoseToPsa(SkeletonAsset Skeleton, List<Vector> bonesOffset, float OverrideScale = 1.0f)
        {
            if (Skeleton == null)
            {
                throw new ArgumentNullException("Provided skeleton was null, cannot proceed with PSA export!");
            }
            
            PSAFile psaFile = new PSAFile();
            psaFile.data.Bones = CreatePskSkeleton(Skeleton, OverrideScale);
            psaFile.data.Infos = new List<PSAFile.PSAAnimInfo>();
            psaFile.data.Keys = new List<PSAFile.PSAAnimKeys>();

            PSAFile.PSAAnimInfo MorphPoseAnimInfo = new PSAFile.PSAAnimInfo();
            MorphPoseAnimInfo.name = "FaceMorph";
            MorphPoseAnimInfo.group = "GroupFaceMorph";
            MorphPoseAnimInfo.TotalBones = Skeleton.Bones.Count;
            MorphPoseAnimInfo.RootInclude = 1;
            MorphPoseAnimInfo.NumRawFrames = 1;
            MorphPoseAnimInfo.AnimRate = 30.0f;

            psaFile.data.Infos.Add(MorphPoseAnimInfo);

            for (int i = 0; i < bonesOffset.Count; i++)
            {
                
                PSAFile.PSAAnimKeys boneKey = new PSAFile.PSAAnimKeys();
                boneKey.rotation = psaFile.data.Bones[i].rotation;

                Vector nBoneLocation = new Vector(new float [3]);
                nBoneLocation.members[0] = Skeleton.Bones[i].Location.members[0] + (bonesOffset[i].members[0] * OverrideScale);
                nBoneLocation.members[2] = Skeleton.Bones[i].Location.members[1] + (bonesOffset[i].members[1] * OverrideScale);
                nBoneLocation.members[1] = Skeleton.Bones[i].Location.members[2] + (bonesOffset[i].members[2] * OverrideScale);
                PSKFile.PSKPoint boneKeyLocation = new PSKFile.PSKPoint(ConvertVector3ToPsk(nBoneLocation));       
                boneKey.location = boneKeyLocation;

                boneKey.time = 1;

                psaFile.data.Keys.Add(boneKey);
            }
            return psaFile.SaveToMemory().ToArray();
        }

        private List<PSKFile.PSKBone> CreatePskSkeleton(SkeletonAsset skeleton, float OverrideScale = 1.0f)
        {
            var PskBones = new List<PSKFile.PSKBone>();
            if (skeleton != null)
            {
                for (int i = 0; i < skeleton.Bones.Count; i++)
                {
                    PSKFile.PSKBone Bone = new PSKFile.PSKBone();
                    Bone.name = skeleton.Bones[i].Name;
                    Bone.childs = skeleton.Bones[i].Children.Count;
                    Bone.parent = (skeleton.Bones[i].ParentIndex == -1) ? 0 : skeleton.Bones[i].ParentIndex;
                    Bone.index = i;
                    Bone.location = new PSKFile.PSKPoint(ConvertVector3ToPsk(skeleton.Bones[i].Location) * OverrideScale);
                    Bone.rotation = new PSKFile.PSKQuad(0, 0, 0, 0);

                    float[][] RotMatrix = new float[4][];
                    RotMatrix[0] = new float[4];
                    RotMatrix[1] = new float[4];
                    RotMatrix[2] = new float[4];
                    RotMatrix[3] = new float[4];

                    RotMatrix[0][0] = skeleton.Bones[i].Right.members[0]; RotMatrix[0][1] = skeleton.Bones[i].Right.members[1]; RotMatrix[0][2] = skeleton.Bones[i].Right.members[2]; RotMatrix[0][3] = 0.0f;
                    RotMatrix[1][0] = skeleton.Bones[i].Up.members[0]; RotMatrix[1][1] = skeleton.Bones[i].Up.members[1]; RotMatrix[1][2] = skeleton.Bones[i].Up.members[2]; RotMatrix[1][3] = 0.0f;
                    RotMatrix[2][0] = skeleton.Bones[i].Forward.members[0]; RotMatrix[2][1] = skeleton.Bones[i].Forward.members[1]; RotMatrix[2][2] = skeleton.Bones[i].Forward.members[2]; RotMatrix[2][3] = 0.0f;
                    RotMatrix[3][0] = 0.0f; RotMatrix[3][1] = 0.0f; RotMatrix[3][2] = 0.0f; RotMatrix[3][3] = 1.0f;

                    Vector Quat = new Vector(new float[4]);
                    float tr = RotMatrix[0][0] + RotMatrix[1][1] + RotMatrix[2][2];
                    float s;

                    if (tr > 0.0f)
                    {
                        float InvS = 1.0f / (float)Math.Sqrt(tr + 1.0f);
                        Quat.members[3] = 0.5f * (1.0f / InvS);
                        s = 0.5f * InvS;

                        Quat.members[0] = (RotMatrix[1][2] - RotMatrix[2][1]) * s;
                        Quat.members[1] = (RotMatrix[2][0] - RotMatrix[0][2]) * s;
                        Quat.members[2] = (RotMatrix[0][1] - RotMatrix[1][0]) * s;
                    }
                    else
                    {
                        int m = 0;
                        if (RotMatrix[1][1] > RotMatrix[0][0])
                            m = 1;

                        if (RotMatrix[2][2] > RotMatrix[m][m])
                            m = 2;

                        int[] nxt = new int[] { 1, 2, 0 };
                        int j = nxt[m];
                        int k = nxt[j];

                        s = RotMatrix[m][m] - RotMatrix[j][j] - RotMatrix[k][k] + 1.0f;
                        float InvS = 1.0f / (float)Math.Sqrt(s);

                        float[] qt = new float[4];
                        qt[m] = 0.5f * (1.0f / InvS);
                        s = 0.5f * InvS;

                        qt[3] = (RotMatrix[j][k] - RotMatrix[k][j]) * s;
                        qt[j] = (RotMatrix[m][j] + RotMatrix[j][m]) * s;
                        qt[k] = (RotMatrix[m][k] + RotMatrix[k][m]) * s;

                        Quat.members[0] = qt[0];
                        Quat.members[1] = qt[1];
                        Quat.members[2] = qt[2];
                        Quat.members[3] = qt[3];
                    }

                    Bone.rotation = new PSKFile.PSKQuad(ConvertVector4ToPsk(Quat));
                    PskBones.Add(Bone);
                }
            }
            return PskBones;
        }

        // conversions to export to psk as psk orientation is different from the one used in frostbite/mea
        // consists in mirroring along z-axis.
        private Vector ConvertVector3ToPsk(Vector vector)
        {
            if (Reverse)
            {
                Vector result = new Vector(vector.members);
                result.members[2] = vector.members[2] * -1;
                return result;
            }
            else return vector;

        }
        private Vector ConvertVector2ToPsk(Vector vector2)
        {
            if (Reverse)
            {
                Vector result = new Vector(vector2.members);
                result.members[1] = -vector2.members[1] + 1;
                return result;
            }
            else return vector2;

        }
        private Vector ConvertVector4ToPsk(Vector vector)
        {
            if (Reverse)
            {
                Vector result = new Vector(vector.members);
                result.members[2] = vector.members[2] * -1;
                result.members[3] = vector.members[3] * -1;
                return result;
            }
            else return vector;
        }
    }
}
