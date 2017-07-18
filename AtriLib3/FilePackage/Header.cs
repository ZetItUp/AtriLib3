using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;

namespace AtriLib3.FilePackage
{
    public class Header
    {
        public int TotalEntries { get; set; }
        private List<long> EntriesSizeList { get; set; }
        public long[] EntriesSize
        {
            get
            {
                return EntriesSizeList.ToArray();
            }
        }

        public Header()
        {
            TotalEntries = 0;
            EntriesSizeList = new List<long>();
        }

        public void AddEntrySize(long newSize)
        {
            EntriesSizeList.Add(newSize);
        }
    }
}
