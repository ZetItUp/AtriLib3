using System;
using System.IO;

namespace AtriLib3
{
    public static class AMathHelper
    {
        public static float FHalf(float num)
        {
            return num * 0.5f;
        }

        public static float FHalf(int num)
        {
            return num / 2;
        }

        public static int Half(float num)
        {
            return (int)(num * 0.5f);
        }

        public static int Half(int num)
        {
            return (int)(num / 2);
        }
    }

    public sealed class Helper
    {
        public enum FileModeType
        {
            CreateNew,
            Create,
            Open,
            OpenOrCreate,
            Truncate,
            Append
        };

        public enum FileAccesser
        {
            Read = 1,
            Write = 2,
            ReadWrite = 3
        };

        public static FileAccess FileAccess(FileAccesser access)
        {
            switch (access)
            {
                case FileAccesser.Read:
                    return System.IO.FileAccess.Read;
                case FileAccesser.ReadWrite:
                    return System.IO.FileAccess.ReadWrite;
                case FileAccesser.Write:
                    return System.IO.FileAccess.Write;
            }

            throw new Exception("No such FileAccess");
        }

        public static FileMode FileMode(FileModeType type)
        {
            switch (type)
            {
                case FileModeType.CreateNew:
                    return System.IO.FileMode.CreateNew;
                case FileModeType.Create:
                    return System.IO.FileMode.Create;
                case FileModeType.Open:
                    return System.IO.FileMode.Open;
                case FileModeType.OpenOrCreate:
                    return System.IO.FileMode.OpenOrCreate;
                case FileModeType.Truncate:
                    return System.IO.FileMode.Truncate;
                case FileModeType.Append:
                    return System.IO.FileMode.Append;

            }

            throw new Exception("No such FileMode");
        }
    }
}
