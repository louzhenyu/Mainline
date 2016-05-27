using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Org.Apache.Jute;
using log4net;

//added by Yang Li at Feb.29th, 2016
namespace Org.Apache.Zookeeper.Proto
{
    public class GetRemarkRequest : IRecord, IComparable
    {
        private static ILog log = LogManager.GetLogger(typeof(GetRemarkRequest));
        public GetRemarkRequest()
        {
        }
        public GetRemarkRequest(string path, bool watch)
        {
            Path = path;
            Watch = watch;
        }
        public string Path { get; set; }
        public bool Watch { get; set; }
        public void Serialize(IOutputArchive a_, String tag)
        {
            a_.StartRecord(this, tag);
            a_.WriteString(Path, "path");
            a_.WriteBool(Watch, "watch");
            a_.EndRecord(this, tag);
        }
        public void Deserialize(IInputArchive a_, String tag)
        {
            a_.StartRecord(tag);
            Path = a_.ReadString("path");
            Watch = a_.ReadBool("watch");
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
                    a_.WriteBool(Watch, "watch");
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
            GetRemarkRequest peer = (GetRemarkRequest)obj;
            if (peer == null)
            {
                throw new InvalidOperationException("Comparing different types of records.");
            }
            int ret = 0;
            ret = Path.CompareTo(peer.Path);
            if (ret != 0) return ret;
            ret = (Watch == peer.Watch) ? 0 : (Watch ? 1 : -1);
            if (ret != 0) return ret;
            return ret;
        }
        public override bool Equals(object obj)
        {
            GetRemarkRequest peer = (GetRemarkRequest)obj;
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
            ret = (Watch == peer.Watch);
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
            ret = (Watch) ? 0 : 1;
            result = 37 * result + ret;
            return result;
        }
        public static string Signature()
        {
            return "LGetRemarkRequest(sz)";
        }
    }
}
