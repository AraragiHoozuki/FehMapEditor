using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FehMapEditor.HSDArc
{

    class ArchiveWriter
    {
        protected Stream stream {
            get
            {
                return writer.BaseStream;
            }
        }
        protected BinaryWriter writer;
        protected Stream ptrs;

        public ArchiveWriter()
        {
            Stream data = new MemoryStream();
            writer = new BinaryWriter(data);
        }

        public void Write(byte value, byte key)
        {
            writer.Write((byte)(value ^ key));
        }
        public void Write(ushort value, ushort key)
        {
            writer.Write((ushort)(value ^ key));
        }
        public void Write(short value, ushort key)
        {
            writer.Write((short)(value ^ key));
        }
        public void Write(uint value, uint key)
        {
            writer.Write((uint)(value ^ key));
        }
        public void Write(ulong value, ulong key)
        {
            writer.Write(value ^ key);
        }

    }
}
