using System.Collections.Generic;

namespace JSOA.Redis
{
    public class ScanResult
    {
        public ulong Cursor { get; set; }
        public List<byte[]> Results { get; set; }
    }
}