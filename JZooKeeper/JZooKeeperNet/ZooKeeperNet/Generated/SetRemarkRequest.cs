using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Org.Apache.Jute;
using log4net;

//added by Yang Li at Feb.29th, 2016
namespace Org.Apache.Zookeeper.Proto
{
    public class SetRemarkRequest : IRecord, IComparable
    {
        private static ILog log = LogManager.GetLogger(typeof(SetRemarkRequest));

        public SetRemarkRequest()
        {
        }
        public SetRemarkRequest(string path, byte[] remark, int version)
        {
            Path = path;
            Remark = remark;
            Version = version;
        }
        public string Path { get; set; }
        public byte[] Remark { get; set; }
        public int Version { get; set; }

        public void Serialize(IOutputArchive a_, String tag)
        {
            a_.StartRecord(this, tag);
            a_.WriteString(Path, "path");
            a_.WriteBuffer(Remark, "remark");
            a_.WriteInt(Version, "version");
            a_.EndRecord(this, tag);
        }
        public void Deserialize(IInputArchive a_, String tag)
        {
            a_.StartRecord(tag);
            Path = a_.ReadString("path");
            Remark = a_.ReadBuffer("remark");
            Version = a_.ReadInt("version");
            a_.EndRecord(tag);
        }

        public override String ToString()
        {
            try
            {
                System.IO.MemoryStream ms = new System.IO.MemoryStream();
                using (ZooKeeperNet.IO.EndianBinaryWriter writer =
                  new ZooKeeperNet.IO.EndianBinaryWriter(ZooKeeperNet.IO.EndianBitConverter.Big, ms, System.Text.Encoding.UTF8))
                {
                    BinaryOutputArchive a_ =
                      new BinaryOutputArchive(writer);
                    a_.StartRecord(this, string.Empty);
                    a_.WriteString(Path, "path");
                    a_.WriteBuffer(Remark, "remark");
                    a_.WriteInt(Version, "version");
                    a_.EndRecord(this, string.Empty);
                    ms.Position = 0;
                    return System.Text.Encoding.UTF8.GetString(ms.ToArray());
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            return "ERROR";
        }

        public void Write(ZooKeeperNet.IO.EndianBinaryWriter writer)
        {
            BinaryOutputArchive archive = new BinaryOutputArchive(writer);
            Serialize(archive, string.Empty);
        }
        public void ReadFields(ZooKeeperNet.IO.EndianBinaryReader reader)
        {
            BinaryInputArchive archive = new BinaryInputArchive(reader);
            Deserialize(archive, string.Empty);
        }

        public int CompareTo(object obj)
        {
            SetRemarkRequest peer = (SetRemarkRequest)obj;
            if (peer == null)
            {
                throw new InvalidOperationException("Comparing different types of records.");
            }
            int ret = 0;
            ret = Path.CompareTo(peer.Path);
            if (ret != 0) return ret;
            ret = Remark.CompareTo(peer.Remark);
            if (ret != 0) return ret;
            ret = (Version == peer.Version) ? 0 : ((Version < peer.Version) ? -1 : 1);
            if (ret != 0) return ret;
            return ret;
        }
        public override bool Equals(object obj)
        {
            SetRemarkRequest peer = (SetRemarkRequest)obj;
            if (peer == null)
            {
                return false;
            }
            if (Object.ReferenceEquals(peer, this))
            {
                return true;
            }
            bool ret = false;
            ret = Path.Equals(peer.Path);
            if (!ret) return ret;
            ret = Remark.Equals(peer.Remark);
            if (!ret) return ret;
            ret = (Version == peer.Version);
            if (!ret) return ret;
            return ret;
        }
        public override int GetHashCode()
        {
            int result = 17;
            int ret = GetType().GetHashCode();
            result = 37 * result + ret;
            ret = Path.GetHashCode();
            result = 37 * result + ret;
            ret = Remark.GetHashCode();
            result = 37 * result + ret;
            ret = (int)Version;
            result = 37 * result + ret;
            return result;
        }
        public static string Signature()
        {
            return "LSetRemarkRequest(sBi)";
        }
    }
}
