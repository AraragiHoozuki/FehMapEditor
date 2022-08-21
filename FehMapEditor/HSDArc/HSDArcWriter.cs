using FehMapEditor.Structs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FehMapEditor.HSDArc
{
    public class HSDArcWriter : BinaryWriter
    {
        public static readonly int HeadSize = 0x20;
        public static readonly byte[] IdXorKey = new byte[] {
          0x81, 0x00, 0x80, 0xA4, 0x5A, 0x16, 0x6F, 0x78,
          0x57, 0x81, 0x2D, 0xF7, 0xFC, 0x66, 0x0F, 0x27,
          0x75, 0x35, 0xB4, 0x34, 0x10, 0xEE, 0xA2, 0xDB,
          0xCC, 0xE3, 0x35, 0x99, 0x43, 0x48, 0xD2, 0xBB,
          0x93, 0xC1
        };

        public HSDArcWriter(Stream output) : base(output)
        {
            pointers = new List<ulong>();
            strings = new List<byte>();
            ptrs = new Dictionary<long, byte[]>();
        }

        public void Write(byte value, byte key)
        {
            Write((byte)(value ^ key));
        }

        public void Write(ushort value, ushort key)
        {
            Write((ushort)(value ^ key));
        }

        public void Write(short value, ushort key)
        {
            Write((short)(value ^ key));
        }

        public void Write(uint value, uint key)
        {
            Write((uint)(value ^ key));
        }

        public void Write(ulong value, ulong key)
        {
            Write(value ^ key);
        }
        public void WriteStats(Stats st)
        {
            Write(st.hp, 0xd632);
            Write(st.atk, 0x14a0);
            Write(st.spd, 0xa55e);
            Write(st.def, 0x8566);
            Write(st.res, 0xaee5);
            Write(new byte[] { 0x57, 0x64, 0x1a, 0x29, 0x59, 0x05 });
        }
        /// <summary>
        /// returns the position before writting
        /// </summary>
        /// <param name="data"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        public long AddPtr(byte[] data, long offset = 0)
        {
            long curr = BaseStream.Position;
            ptrs.Add(curr, data);
            Write(offset);
            return curr;
        }

        public void WriteLongAt(long data, long offset)
        {
            long curr = BaseStream.Position;
            BaseStream.Seek(offset, SeekOrigin.Begin);
            Write(data);
            BaseStream.Seek(curr, SeekOrigin.Begin);
        }

        public void RegisterIdString(string id)
        {
            if (id == null)
            {
                Write((ulong)0);
                return;
            }
            byte[] codes = Encoding.UTF8.GetBytes(id);
            byte[] xored = new byte[(codes.Length + 8) / 8 * 8];
            for (int i = 0; i < codes.Length; i++)
            {
                if (codes[i] != IdXorKey[i % IdXorKey.Length])
                {
                    xored[i] = (byte)(codes[i] ^ IdXorKey[i % IdXorKey.Length]);
                }
                else
                {
                    xored[i] = codes[i];
                }
            }
            strings.AddRange(xored);
            pointers.Add((ulong)(BaseStream.Position - HeadSize));
            Write((ulong)(string_pos - HeadSize));
            string_pos += xored.Length;
        }

        public void IdString(string id)
        {
            if (id == null)
            {
                Write((ulong)0);
                return;
            }
            byte[] codes = Encoding.UTF8.GetBytes(id);
            byte[] xored = new byte[(codes.Length + 8) / 8 * 8];
            for (int i = 0; i < codes.Length; i++)
            {
                if (codes[i] != IdXorKey[i % IdXorKey.Length])
                {
                    xored[i] = (byte)(codes[i] ^ IdXorKey[i % IdXorKey.Length]);
                }
                else
                {
                    xored[i] = codes[i];
                }
            }
            AddPtr(xored);
        }

        protected List<ulong> pointers;
        protected List<byte> strings;
        protected Dictionary<long, byte[]> ptrs;
        protected int string_pos = 0;
    }

    public class SRPGMapWriter : HSDArcWriter
    {

        public static byte[] WriteBin(MapArc arc)
        {
            FileStream fs = File.OpenWrite(arc.path + ".bin");
            SRPGMapWriter smw = new SRPGMapWriter(fs);
            smw.WriteMap(arc);
            smw.Flush();
            smw.Close();
            return File.ReadAllBytes(arc.path + ".bin");
        }

        public SRPGMapWriter(Stream output) : base(output) {
        }
        public void WriteMap(MapArc arc)
        {
            SRPGMap data = arc.Map;

            Seek(HeadSize, SeekOrigin.Begin);
            Write(data.unknown);
            Write(data.highest_score, 0xa9e250b1);
            long field_ptr_pos = AddPtr(null); // field offset
            long player_ptr_pos = AddPtr(null); // player pos offset
            long units_ptr_pos = AddPtr(null); // units offset
            Write((ushort)data.player_pos.Length, 0x9d63c79a);
            Write((ushort)data.units.Length, 0xac6710ee); data.unit_count = (uint)data.units.Length;
            Write(data.turns_to_win, 0xfd);
            Write(data.last_enemy_phase, 0xc7);
            Write(data.turns_to_defend, 0xec);
            Write(new byte[5]);


            //field
            WriteLongAt(BaseStream.Position - HeadSize, field_ptr_pos);
            IdString(data.field.id);
            Write(data.field.width, 0x6b7cd75f);
            Write(data.field.height, 0x2baa12d5);
            Write(data.field.base_terrain, 0x41);
            Write(data.field.padding); // 7 bytes padding
            foreach (byte[] line in data.field.terrain)
            {
                foreach (byte tile in line)
                {
                    Write(tile, 0xA1);
                }
            }

            //player position
            WriteLongAt(BaseStream.Position - HeadSize, player_ptr_pos);
            foreach (Position p in data.player_pos)
            {
                Write(p.x, 0xb332);
                Write(p.y, 0x28b2);
                Write(new byte[4]);
            }

            //units
            WriteLongAt(BaseStream.Position - HeadSize, units_ptr_pos);
            foreach (Unit u in data.units)
            {
                WriteUnit(u);
            }

            //pointed data
            foreach (KeyValuePair<long, byte[]> p in ptrs)
            {
                if (p.Value == null) continue;
                WriteLongAt(BaseStream.Position - HeadSize, p.Key);
                /*long pointed_pos = BaseStream.Position;
                BaseStream.Seek(p.Key, SeekOrigin.Begin);
                Write(pointed_pos - HeadSize);
                BaseStream.Seek(pointed_pos, SeekOrigin.Begin);*/
                Write(p.Value);
            }

            //pointer offsets
            long offset_ptrs = BaseStream.Position;
            foreach (KeyValuePair<long, byte[]> p in ptrs)
            {
                Write(p.Key - HeadSize);
            }
            arc.ptr_offset = ptrs.Keys.Select(offset => (ulong)(offset - HeadSize)).ToArray();

            int total = (int)BaseStream.Position;

            Seek(0, SeekOrigin.Begin);
            Write(total); arc.archive_size = (uint)total;
            Write((int)(offset_ptrs - HeadSize)); arc.ptr_list_offset = (uint)(offset_ptrs - HeadSize);
            Write(ptrs.Count); arc.ptr_list_length = (uint)ptrs.Count;
            Write(arc.ptr_taglist_length);//tag list length
            Write(arc.unknown1);
            Write(arc.unknown2);
            Write(arc.magic);
        }


        public void WriteUnit(Unit u)
        {
            IdString(u.id_tag);
            foreach (string sk in u.skills)
                IdString(sk);
            IdString(u.accessory);
            Write(u.pos.x, 0xb332);
            Write(u.pos.y, 0x28b2);
            Write(u.rarity, 0x61);
            Write(u.lv, 0x2a);
            Write(u.cd, 0x1e);
            Write(u.unknown);
            WriteStats(u.stats);
            Write(u.start_turn, 0xcf);
            Write(u.movement_group, 0xf4);
            Write(u.movement_delay, 0x95);
            Write(u.break_terrain, 0x71);
            Write(u.tether, 0xb8);
            Write(u.true_lv, 0x85);
            Write(u.is_enemy, 0xd0);
            Write(u.padding);
            IdString(u.spawn_check);
            Write(u.spawn_count, 0x0a);
            Write(u.spawn_turns, 0x0a);
            Write(u.spawn_target_remain, 0x2d);
            Write(u.spawn_target_kills, 0x58);
            Write(u.paddings);
        }
    }
}
