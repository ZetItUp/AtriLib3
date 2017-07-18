using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Runtime.InteropServices;
using HelperCls = AtriLib3.Helper;

namespace AtriLib3.FilePackage
{
    public class FilePackage
    {
        public Header FileHeaderDefOutition { get; private set; }
        public FileInfo PackedFile { get; private set; }
        public List<Item> Data { get; private set; }

        public FilePackage(string fileName)
        {
            PackedFile = new FileInfo(fileName);
            FileHeaderDefOutition = new Header();
            Data = new List<Item>();
        }

        public bool PackDirectoryContent(string directoryPath)
        {
            bool retVal = false;

            DirectoryInfo dirInfo = new DirectoryInfo(directoryPath);

            // TODO: Add more error checks etc?

            if (dirInfo.Exists)
            {
                FileInfo[] files = dirInfo.GetFiles("*", SearchOption.AllDirectories);

                foreach (FileInfo fi in files)
                {
                    Item item = SetItem(fi, dirInfo.FullName);

                    if (item != null)
                    {
                        Data.Add(item);
                        FileHeaderDefOutition.TotalEntries++;
                        FileHeaderDefOutition.AddEntrySize(item.ItemSize);
                    }
                }
            }

            retVal = true;

            return retVal;
        }

        private Item SetItem(FileInfo sourceFile, string packedRoot)
        {
            if (sourceFile.Exists)
            {
                Item retVal = new Item();
                retVal.Name = sourceFile.Name;
                retVal.RelativeUri = sourceFile.FullName.Substring(packedRoot.Length).Replace("\\", "/");
                retVal.RawData = File.ReadAllBytes(sourceFile.FullName);

                return retVal;
            }
            else
            {
                return null;
            }
        }

        public void SaveFileEncrypted(CryptoStream cs)
        {
            // Write header ( 4 bytes )
            cs.Write(BitConverter.GetBytes(FileHeaderDefOutition.TotalEntries), 0, 4);

            // 8 bytes entries size
            foreach (long size in FileHeaderDefOutition.EntriesSize)
            {
                cs.Write(BitConverter.GetBytes(size), 0, 8);
            }

            foreach (Item item in Data)
            {
                cs.Write(item.SerializedData, 0, item.SerializedData.Length);
            }

            cs.Close();
        }

        public void SaveFile()
        {
            if (PackedFile.Exists)
            {
                PackedFile.Delete();
                System.Threading.Thread.Sleep(100);
            }

            using (FileStream fs = new FileStream(PackedFile.FullName, FileMode.CreateNew, FileAccess.Write))
            {
                // Write header ( 4 bytes )
                fs.Write(BitConverter.GetBytes(FileHeaderDefOutition.TotalEntries), 0, 4);

                // 8 bytes entries size
                foreach (long size in FileHeaderDefOutition.EntriesSize)
                {
                    fs.Write(BitConverter.GetBytes(size), 0, 8);
                }

                foreach (Item item in Data)
                {
                    fs.Write(item.SerializedData, 0, item.SerializedData.Length);
                }

                fs.Close();
            }
        }

        public void OpenFileEncrypted(string gameKey)
        {
            if (PackedFile.Exists)
            {
                GCHandle gch = GCHandle.Alloc(gameKey, GCHandleType.Pinned);

                DESCryptoServiceProvider DES = new DESCryptoServiceProvider();
                DES.Key = Encoding.ASCII.GetBytes(gameKey);
                DES.IV = Encoding.ASCII.GetBytes(gameKey);

                FileStream fs = new FileStream(PackedFile.FullName, HelperCls.FileMode(HelperCls.FileModeType.Open), HelperCls.FileAccess(HelperCls.FileAccesser.Read));

                ICryptoTransform desdecrypt = DES.CreateDecryptor();
                CryptoStream cs = new CryptoStream(fs, desdecrypt, CryptoStreamMode.Read);

                byte[] readBuffer = new byte[4];
                cs.Read(readBuffer, 0, readBuffer.Length);
                FileHeaderDefOutition.TotalEntries = BitConverter.ToInt32(readBuffer, 0);

                for (int i = 0; i < FileHeaderDefOutition.TotalEntries; i++)
                {
                    readBuffer = new byte[8];
                    cs.Read(readBuffer, 0, readBuffer.Length);
                    FileHeaderDefOutition.AddEntrySize(BitConverter.ToInt64(readBuffer, 0));
                }

                foreach (long size in FileHeaderDefOutition.EntriesSize)
                {
                    readBuffer = new byte[size];
                    cs.Read(readBuffer, 0, readBuffer.Length);
                    Data.Add(new Item(readBuffer));
                }

                cs.Close();
                gch.Free();
            }
        }

        public void OpenFile()
        {
            if (PackedFile.Exists)
            {
                using (FileStream fs = new FileStream(PackedFile.FullName, FileMode.Open, FileAccess.Read))
                {
                    byte[] readBuffer = new byte[4];
                    fs.Read(readBuffer, 0, readBuffer.Length);
                    FileHeaderDefOutition.TotalEntries = BitConverter.ToInt32(readBuffer, 0);

                    for (int i = 0; i < FileHeaderDefOutition.TotalEntries; i++)
                    {
                        readBuffer = new byte[8];
                        fs.Read(readBuffer, 0, readBuffer.Length);
                        FileHeaderDefOutition.AddEntrySize(BitConverter.ToInt64(readBuffer, 0));
                    }

                    foreach (long size in FileHeaderDefOutition.EntriesSize)
                    {
                        readBuffer = new byte[size];
                        fs.Read(readBuffer, 0, readBuffer.Length);
                        Data.Add(new Item(readBuffer));
                    }

                    fs.Close();
                }
            }
        }
    }
}
