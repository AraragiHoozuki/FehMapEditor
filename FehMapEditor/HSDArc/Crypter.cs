using DSDecmp.Formats.Nitro;
using System;
using System.IO;
using System.Linq;

namespace FehMapEditor.HSDArc
{
    public class Crypter
    {
        private static byte[] LZ11Compress(byte[] decompressed)
        {
            using (MemoryStream dstream = new MemoryStream(decompressed))
            {
                using (MemoryStream cstream = new MemoryStream())
                {
                    _ = (new LZ11()).Compress(dstream, decompressed.Length, cstream);
                    return cstream.ToArray();
                }
            }
        }

        private static byte[] LZ11Decompress(byte[] compressed)
        {
            using (MemoryStream cstream = new MemoryStream(compressed))
            {
                using (MemoryStream dstream = new MemoryStream())
                {
                    _ = (new LZ11()).Decompress(cstream, compressed.Length, dstream);
                    return dstream.ToArray();
                }
            }
        }

        public static byte[] EncryptAndCompress(byte[] filedata)
        {
            uint xorkey = BitConverter.ToUInt32(filedata, 0) >> 8;
            xorkey *= 0x8083;
            byte[] lz = filedata.Skip(4).ToArray();
            lz = LZ11Compress(lz);
            byte[] output = new byte[4 + lz.Length + (lz.Length % 4 == 0 ? 0 : 4 - lz.Length % 4)];
            filedata.Take(4).ToArray().CopyTo(output, 0);
            lz.CopyTo(output, 4);
            for (int i = 8; i < output.Length; i += 0x4)
            {
                BitConverter.GetBytes(BitConverter.ToUInt32(output, i) ^ xorkey).CopyTo(output, i);
                xorkey = BitConverter.ToUInt32(output, i);
            }
            return output;
        }

        public static byte[] ReadLZ(string path)
        {
            byte[] filedata = File.ReadAllBytes(path);
            var xorkey = BitConverter.ToUInt32(filedata, 0) >> 8;
            xorkey *= 0x8083;
            for (int i = 8; i < filedata.Length; i += 0x4)
            {
                BitConverter.GetBytes(BitConverter.ToUInt32(filedata, i) ^ xorkey).CopyTo(filedata, i);
                xorkey ^= BitConverter.ToUInt32(filedata, i);
            }
            filedata = filedata.Skip(4).ToArray();
            return LZ11Decompress(filedata);
        }
    }
}
