using FehMapEditor.Structs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FehMapEditor.HSDArc
{
    public class MapArc : FEHArchive
    {

        public MapArc(string path) : base(path) { }

        public SRPGMap Map { get; private set; }
        protected override void ReadData()
        {
            Map = new SRPGMap()
            {
                unknown = stream.ReadUInt32(),
                highest_score = ReadXored32(0xA9E250B1),
                addr_field = stream.ReadUInt64(),
                addr_player_pos = stream.ReadUInt64(),
                addr_units = stream.ReadUInt64(),
                player_count = ReadXored32(0x9D63C79A),
                unit_count = ReadXored32(0xAC6710EE),
                turns_to_win = ReadXored8(0xFD),
                last_enemy_phase = ReadXored8(0xC7),
                turns_to_defend = ReadXored8(0xEC),
                padding = stream.ReadBytes(5),
                field = ReadField(),
            };
            Map.player_pos = ReadPlayerPositions();
            Map.units = ReadUnits();
        }

        private Field ReadField()
        {
            Field f = new Field()
            {
                id = ReadCryptedString(),
                width = ReadXored32(0x6B7CD75F),
                height = ReadXored32(0x2BAA12D5),
                base_terrain = ReadXored8(0x41),
                padding = stream.ReadBytes(7),
            };
            byte[][] tiles = new byte[f.height][];
            for(int i = 0; i < f.height; i++)
            {
                tiles[i] = new byte[f.width];
                for(int j = 0; j < f.width; j++)
                {
                    tiles[i][j] = ReadXored8(0xA1);
                }
            }
            f.terrain = tiles;
            return f;
        }

        private Position[] ReadPlayerPositions()
        {
            stream.BaseStream.Seek((long)Map.addr_player_pos + OffSet, System.IO.SeekOrigin.Begin);
            Position[] ps = new Position[Map.player_count];
            if (Map.player_count > 0)
            {
                for(int i = 0; i < Map.player_count; i++)
                {
                    ps[i] = new Position()
                    {
                        x = ReadXored16(0xB332),
                        y = ReadXored16(0x28B2)
                    };
                }
            }
            return ps;
        }

        private Unit[] ReadUnits()
        {
            stream.BaseStream.Seek((long)Map.addr_units + OffSet, System.IO.SeekOrigin.Begin);
            Unit[] units = new Unit[Map.unit_count];
            for (int i = 0; i < Map.unit_count; i++)
            {
                units[i] = new Unit()
                {
                    id_tag = ReadCryptedString(),
                    skills = new string[7]
                    {
                        ReadCryptedString(),
                        ReadCryptedString(),
                        ReadCryptedString(),
                        ReadCryptedString(),
                        ReadCryptedString(),
                        ReadCryptedString(),
                        ReadCryptedString(),
                    },
                    accessory = ReadCryptedString(),
                    pos = new Position()
                    {
                        x = ReadXored16(0xB332),
                        y = ReadXored16(0x28B2)
                    },
                    rarity = ReadXored8(0x61),
                    lv = ReadXored8(0x2A),
                    cd = ReadXored8(0x1E),
                    unknown = stream.ReadByte(),
                    stats = ReadStats(),
                    start_turn = ReadXored8(0xCF),
                    movement_group = ReadXored8(0xF4),
                    movement_delay = ReadXored8(0x95),
                    break_terrain = ReadXored8(0x71),
                    tether = ReadXored8(0xB8),
                    true_lv = ReadXored8(0x85),
                    is_enemy = ReadXored8(0xD0),
                    padding = stream.ReadByte(),
                    spawn_check = ReadCryptedString(),
                    spawn_count = ReadXored8(0x0A),
                    spawn_turns = ReadXored8(0x0A),
                    spawn_target_remain = ReadXored8(0x2D),
                    spawn_target_kills = ReadXored8(0x5B),
                    paddings = stream.ReadBytes(4)
                };
            }
            return units;
        }
    }
}
