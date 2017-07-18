using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;

namespace AtriLib3.FilePackage
{
    public class Item
    {
        public byte[] RawData { get; set; }
        public string Name { get; set; }
        public string RelativeUri { get; set; }

        public long ItemSize
        {
            get
            {
                long retVal = 4;    // Name length;
                retVal += Name.Length;
                retVal += 4;        // RelativeUri length
                retVal += RelativeUri.Length;
                retVal += RawData.Length;

                return retVal;
            }
        }

        public byte[] SerializedData
        {
            get
            {
                List<byte> retVal = new List<byte>();
                retVal.AddRange(BitConverter.GetBytes(Name.Length));
                retVal.AddRange(Encoding.Default.GetBytes(Name));
                retVal.AddRange(BitConverter.GetBytes(RelativeUri.Length));
                retVal.AddRange(Encoding.Default.GetBytes(RelativeUri));
                retVal.AddRange(RawData);

                return retVal.ToArray();
            }
        }

        public Item()
        {
            RawData = new byte[0];
            Name = string.Empty;
            RelativeUri = string.Empty;
        }

        public Item(byte[] serializedItem)
        {
            int cursor = 0;
            int nl = BitConverter.ToInt32(serializedItem, cursor);
            cursor += 4;
            Name = Encoding.Default.GetString(serializedItem, cursor, nl);
            cursor += nl;
            int rl = BitConverter.ToInt32(serializedItem, cursor);
            cursor += 4;
            RelativeUri = Encoding.Default.GetString(serializedItem, cursor, rl);
            cursor += rl;
            RawData = new byte[serializedItem.Length - cursor];

            for (int i = cursor; i < serializedItem.Length; i++)
            {
                RawData[i - cursor] = serializedItem[i];
            }
        }
    }
}
