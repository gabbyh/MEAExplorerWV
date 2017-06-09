using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PluginSystem
{
    public class PSAFile
    {
        #region Declaration

        public struct PSAAnimInfo
        {
            public byte[] raw;
            public string name;
            public string group;
            public int TotalBones;
            public int RootInclude;
            public int KeyCompressionStyle;
            public int KeyQuotum;
            public float KeyReduction;
            public float TrackTime;
            public float AnimRate;
            public int StartBone;
            public int FirstRawFrame;
            public int NumRawFrames;
        }

        public struct PSAAnimKeys
        {
            public byte[] raw;
            public PSKFile.PSKPoint location;
            public PSKFile.PSKQuad rotation;
            public float time;
        }

        public struct PSAData
        {
            public List<PSKFile.PSKBone> Bones;
            public List<PSAAnimInfo> Infos;
            public List<PSAAnimKeys> Keys;
        }

        public struct ChunkHeader
        {
            public string name;
            public int flags;
            public int size;
            public int count;
        }

        public PSAData data;

#endregion

        public PSAFile()
        {
        }

        public void ExportPSA(string path)
        {           
            FileStream fs = new FileStream(path, FileMode.Create, FileAccess.Write);
            Save(fs);
            fs.Close();
        }

        public MemoryStream SaveToMemory()
        {
            MemoryStream m = new MemoryStream();
            Save(m);
            return m;
        }

        public void Save(Stream m)
        {
            WriteAnimHead(m);
            WriteBones(m);
            WriteInfos(m);
            WriteKeys(m);
        }

        public void WriteHeader(Stream m, string name, int size, int count)
        {
            for (int i = 0; i < 20; i++)
                if (i < name.Length)
                    m.WriteByte((byte)name[i]);
                else
                    m.WriteByte(0);
            m.Write(BitConverter.GetBytes(0x1e83b9), 0, 4);
            m.Write(BitConverter.GetBytes(size), 0, 4);
            m.Write(BitConverter.GetBytes(count), 0, 4);
        }

        public void WriteAnimHead(Stream m)
        {
            WriteHeader(m, "ANIMHEAD", 0, 0);
        }

        public void WriteBones(Stream m)
        {
            WriteHeader(m, "BONENAMES", 0x78, data.Bones.Count);
            foreach (PSKFile.PSKBone b in data.Bones)
            {
                for (int i = 0; i < 64; i++)
                    if (i < b.name.Length)
                        m.WriteByte((byte)b.name[i]);
                    else
                        m.WriteByte(0);
                m.Write(BitConverter.GetBytes(0), 0, 4);
                m.Write(BitConverter.GetBytes(b.childs), 0, 4);
                m.Write(BitConverter.GetBytes(b.parent), 0, 4);
                for (int i = 0; i < 44; i++)
                    m.WriteByte(0);
            }
        }

        public void WriteInfos(Stream m)
        {
            WriteHeader(m, "ANIMINFO", 0xa8, data.Infos.Count);
            foreach (PSAAnimInfo inf in data.Infos)
            {
                for (int i = 0; i < 64; i++)
                    if (i < inf.name.Length)
                        m.WriteByte((byte)inf.name[i]);
                    else
                        m.WriteByte(0);
                for (int i = 0; i < 64; i++)
                    if (i < inf.group.Length)
                        m.WriteByte((byte)inf.group[i]);
                    else
                        m.WriteByte(0);
                m.Write(BitConverter.GetBytes(inf.TotalBones), 0, 4);
                m.Write(BitConverter.GetBytes(inf.RootInclude), 0, 4);
                m.Write(BitConverter.GetBytes(inf.KeyCompressionStyle), 0, 4);
                m.Write(BitConverter.GetBytes(inf.KeyQuotum), 0, 4);
                m.Write(BitConverter.GetBytes(inf.KeyReduction), 0, 4);
                m.Write(BitConverter.GetBytes(inf.TrackTime), 0, 4);
                m.Write(BitConverter.GetBytes(inf.AnimRate), 0, 4);
                m.Write(BitConverter.GetBytes(inf.StartBone), 0, 4);
                m.Write(BitConverter.GetBytes(inf.FirstRawFrame), 0, 4);
                m.Write(BitConverter.GetBytes(inf.NumRawFrames), 0, 4);
            }
        }

        public void WriteKeys(Stream m)
        {
            WriteHeader(m, "ANIMKEYS", 0x20, data.Keys.Count);
            foreach (PSAAnimKeys k in data.Keys)
            {
                m.Write(BitConverter.GetBytes(k.location.x), 0, 4);
                m.Write(BitConverter.GetBytes(k.location.y), 0, 4);
                m.Write(BitConverter.GetBytes(k.location.z), 0, 4);
                m.Write(BitConverter.GetBytes(k.rotation.x), 0, 4);
                m.Write(BitConverter.GetBytes(k.rotation.y), 0, 4);
                m.Write(BitConverter.GetBytes(k.rotation.z), 0, 4);
                m.Write(BitConverter.GetBytes(k.rotation.w), 0, 4);
                m.Write(BitConverter.GetBytes(k.time), 0, 4);
            }
        }
    }
}
