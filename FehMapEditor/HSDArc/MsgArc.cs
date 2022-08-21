using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FehMapEditor.HSDArc
{
    public class MsgArc : FEHArchive
    {
        protected static new readonly byte[] IdKey = {
          0x6F, 0xB0, 0x8F, 0xD6, 0xEF, 0x6A, 0x5A, 0xEB, 0xC6, 0x76, 0xF6, 0xE5,
          0x56, 0x9D, 0xB8, 0x08, 0xE0, 0xBD, 0x93, 0xBA, 0x05, 0xCC, 0x26, 0x56,
          0x65, 0x1E, 0xF8, 0x2B, 0xF9, 0xA1, 0x7E, 0x41, 0x18, 0x21, 0xA4, 0x94,
          0x25, 0x08, 0xB8, 0x38, 0x2B, 0x98, 0x53, 0x76, 0xC6, 0x2E, 0x73, 0x5D,
          0x74, 0xCB, 0x02, 0xE8, 0x98, 0xAB, 0xD0, 0x36, 0xE5, 0x37
        };

        public Dictionary<string, string> Items { get; } = new();
        public MsgArc(string path) : base(path) { }

        protected new string ReadCryptedString()
        {
            ulong str_addr = stream.ReadUInt64();
            long pos = stream.BaseStream.Position;
            byte[] enc;
            if (str_addr != 0)
            {
                stream.BaseStream.Seek((long)(OffSet + str_addr), SeekOrigin.Begin);
                enc = ReadTilNull();
                stream.BaseStream.Seek(pos, SeekOrigin.Begin);
                for (int i = 0; i < enc.Length; i++)
                {
                    if (enc[i] != IdKey[i % IdKey.Length]) enc[i] ^= IdKey[i % IdKey.Length];
                }
                return Encoding.UTF8.GetString(enc);
            }
            return null;
        }

        protected override void ReadData()
        {
            list_size = stream.ReadUInt64();
            ReadListItems();
        }

        protected void ReadListItems()
        {
            for(ulong i = 0; i < list_size; i++)
            {
                Items.Add(ReadCryptedString(), ReadCryptedString());
            }
        }
    }
}
