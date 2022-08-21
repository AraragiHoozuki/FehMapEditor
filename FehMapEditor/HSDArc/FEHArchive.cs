using FehMapEditor.Structs;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace FehMapEditor.HSDArc
{
    public class FEHArchive : System.IDisposable
    {
        protected static readonly byte[] IdKey = {
            0x81, 0x00, 0x80, 0xA4, 0x5A, 0x16, 0x6F, 0x78,
            0x57, 0x81, 0x2D, 0xF7, 0xFC, 0x66, 0x0F, 0x27,
            0x75, 0x35, 0xB4, 0x34, 0x10, 0xEE, 0xA2, 0xDB,
            0xCC, 0xE3, 0x35, 0x99, 0x43, 0x48, 0xD2, 0xBB,
            0x93, 0xC1
        };
        protected static readonly uint OffSet = 0x20;
        public string path;
        protected BinaryReader stream;

        public uint archive_size;
        public uint ptr_list_offset;
        public uint ptr_list_length;
        public uint ptr_taglist_length;
        public uint unknown1;
        public uint unknown2;
        public ulong magic;
        public ulong[] ptr_offset;
        public ulong[] tag_list;
        public char[] tags;

        public ulong list_addr;
        public ulong list_size = 0;

        public FEHArchive(string path)
        {
            this.path = path;
            if (Path.GetExtension(path).ToLower() == ".lz")
            {
                stream = new BinaryReader(new MemoryStream(Crypter.ReadLZ(path)));
            }
            else
            {
                stream = new BinaryReader(new FileStream(path, FileMode.Open));
            }
            ReadAll();
        }


        public byte ReadXored8(byte key)
        {
            return (byte)(stream.ReadByte() ^ key);
        }
        public ushort ReadXored16(ushort key)
        {
            return (ushort)(stream.ReadUInt16() ^ key);
        }
        public uint ReadXored32(uint key)
        {
            return stream.ReadUInt32() ^ key;
        }
        public ulong ReadXored64(ulong key)
        {
            return stream.ReadUInt64() ^ key;
        }
        protected byte[] ReadTilNull()
        {
            List<byte> list = new List<byte>();
            byte reading = stream.ReadByte();
            while (reading != 0)
            {
                list.Add(reading);
                reading = stream.ReadByte();
            }
            return list.ToArray();
        }
        protected string ReadCryptedString()
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

        protected string ReadString()
        {
            ulong str_addr = stream.ReadUInt64();
            long pos = stream.BaseStream.Position;
            byte[] enc;
            if (str_addr != 0)
            {
                stream.BaseStream.Seek((long)(OffSet + str_addr), SeekOrigin.Begin);
                enc = ReadTilNull();
                stream.BaseStream.Seek(pos, SeekOrigin.Begin);
                return Encoding.UTF8.GetString(enc);
            }
            return null;
        }

        protected Stats ReadStats()
        {
            Stats stat = new Stats
            {
                hp = ReadXored16(0xD632),
                atk = ReadXored16(0x14A0),
                spd = ReadXored16(0xA55E),
                def = ReadXored16(0x8566),
                res = ReadXored16(0xAEE5)
            };
            stream.BaseStream.Seek(6, SeekOrigin.Current);
            return stat;
        }

        protected void ReadHead()
        {
            archive_size = stream.ReadUInt32();
            ptr_list_offset = stream.ReadUInt32();
            ptr_list_length = stream.ReadUInt32();
            ptr_taglist_length = stream.ReadUInt32();
            unknown1 = stream.ReadUInt32();
            unknown2 = stream.ReadUInt32();
            magic = stream.ReadUInt64();
        }

        public void ReadAll()
        {
            ReadHead();
            ReadData();
            ReadTail();
        }

        protected virtual void ReadData()
        {

        }

        protected void ReadTail()
        {
            if (ptr_list_offset > 0)
            {
                stream.BaseStream.Seek(OffSet + ptr_list_offset, SeekOrigin.Begin);
                if (ptr_list_length > 0)
                {
                    ptr_offset = new ulong[ptr_list_length];
                    for (int i = 0; i < ptr_list_length; i++)
                    {
                        ptr_offset[i] = stream.ReadUInt64();
                    }
                }
            }
        }

        public void Dispose()
        {
            stream.Dispose();
        }
    }
}
