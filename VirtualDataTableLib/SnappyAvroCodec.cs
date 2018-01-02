﻿using Snappy;
using System.IO;
using System.IO.Compression;

namespace Avro.File
{
    public class SnappyAvroCodec : Codec
    {
        public override byte[] Compress(byte[] uncompressedData)
        {
            var outStream = new MemoryStream();

            using (var compress = new SnappyStream(outStream, CompressionMode.Compress))
            {
                compress.Write(uncompressedData, 0, uncompressedData.Length);
            }
            return outStream.ToArray();
        }

        public override byte[] Decompress(byte[] compressedData)
        {
            var inStream = new MemoryStream(compressedData);
            var outStream = new MemoryStream();

            using (var decompress = new SnappyStream(inStream, CompressionMode.Decompress))
            {
                CopyTo(decompress, outStream);
            }
            return outStream.ToArray();
        }

        public override bool Equals(object other)
        {
            if (this == other)
                return true;
            return (this.GetType().Name == other.GetType().Name);
        }

        public override int GetHashCode()
        {
            return 1003;
        }

        public override string GetName()
        {
            return "snappy";
        }

        #region Private Helpers

        private static void CopyTo(Stream from, Stream to)
        {
            byte[] buffer = new byte[4096];
            int read;
            while ((read = from.Read(buffer, 0, buffer.Length)) != 0)
            {
                to.Write(buffer, 0, read);
            }
        }
        
        #endregion Private Helpers
    }
}
