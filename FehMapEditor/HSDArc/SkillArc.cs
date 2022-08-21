using FehMapEditor.Structs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FehMapEditor.HSDArc
{
    public class SkillArc : FEHArchive
    {
        protected static readonly ulong ListKey = 0x7FECC7074ADEE9AD;

        public List<Skill> Items { get; } = new List<Skill>();

        public SkillArc(string path) : base(path) { }

        protected override void ReadData()
        {
            list_addr = stream.ReadUInt64();
            list_size = ReadXored64(ListKey);
            ReadListItems();
        }

        protected void ReadListItems()
        {
            for (ulong i = 0; i < list_size; i++)
            {
                Skill p = new Skill
                {
                    id = ReadCryptedString(),
                    refine_base = ReadCryptedString(),
                    name = ReadCryptedString(),
                    description = ReadCryptedString(),
                    refine_id = ReadCryptedString(),
                    beast_effect_id = ReadCryptedString(),
                    requirements = new string[2] { ReadCryptedString(), ReadCryptedString() },
                    next_skill = ReadCryptedString(),
                    sprites = new string[4] { ReadString(), ReadString(), ReadString(), ReadString() },
                    stats = ReadStats(),
                    class_params = ReadStats(),
                    combat_buffs = ReadStats(),
                    skill_params = ReadStats(),
                    skill_params2 = ReadStats(),
                    refine_stats = ReadStats(),
                    id_num = ReadXored32(0xC6A53A23),
                    sort_value = ReadXored32(0x8DDBF8AC),
                    icon = ReadXored32(0xC6DF2173),
                    wep_equip = ReadXored32(0x35B99828),
                    mov_equip = ReadXored32(0xAB2818EB),
                    sp_cost = ReadXored32(0xC031F669),
                    category = (SkillCategory)ReadXored8(0xBC),
                    tome_class = (Element)ReadXored8(0x35),
                    is_exclusive = ReadXored8(0xCC),
                    enemy_only = ReadXored8(0x4F),
                    range = ReadXored8(0x56),
                    might = ReadXored8(0xD2),
                    cooldown = ReadXored8(0x56),
                    assist_cd = ReadXored8(0xF2),
                    healing = ReadXored8(0x95),
                    skill_range = ReadXored8(0x09),
                    score = ReadXored16(0xA232),
                    promotion_tier = ReadXored8(0xE0),
                    promotion_rarity = ReadXored8(0x75),
                    is_refined = ReadXored8(0x02),
                    refine_sort_id = ReadXored8(0xFC),
                    tokkou_wep = ReadXored32(0x23BE3D43),
                    tokkou_mov = ReadXored32(0x823FDAEB),
                    shield_wep = ReadXored32(0xAABAB743),
                    shield_mov = ReadXored32(0x0EBEF25B),
                    weak_wep = ReadXored32(0x005A02AF),
                    weak_mov = ReadXored32(0xB269B819),
                    unknown1 = stream.ReadUInt32(),
                    unknown2 = stream.ReadUInt32(),
                    adaptive_wep = ReadXored32(0x494E2629),
                    adaptive_mov = ReadXored32(0xEE6CEF2E),
                    timing = ReadXored32(0x9C776648),
                    ability = ReadXored32(0x72B07325),
                    limit1 = ReadXored32(0x0EBDB832),
                    limit1_params = new ushort[2] { ReadXored16(0xA590), ReadXored16(0xA590) },
                    limit2 = ReadXored32(0x0EBDB832),
                    limit2_params = new ushort[2] { ReadXored16(0xA590), ReadXored16(0xA590) },
                    target_wep = ReadXored32(0x409FC9D7),
                    target_mov = ReadXored32(0x6C64D122),
                    passive_next = ReadCryptedString(),
                    timestamp = ReadXored64(0xED3F39F93BFE9F51),
                    random_allowed = ReadXored8(0x10),
                    min_lv = ReadXored8(0x90),
                    max_lv = ReadXored8(0x24),
                    tt_inherit_base = ReadXored8(0x19),
                    random_mode = ReadXored8(0xBE),
                    padding1 = stream.ReadBytes(3),
                    limit3 = ReadXored32(0x0EBDB832),
                    limit3_params = new ushort[2] { ReadXored16(0xA590), ReadXored16(0xA590) },
                    range_shape = ReadXored8(0x5C),
                    target_either = ReadXored8(0xA7),
                    distant_counter = ReadXored8(0xDB),
                    canto = ReadXored8(0x41),
                    pathfinder = ReadXored8(0xBE),
                    padding2 = stream.ReadBytes(3)
                };
                Items.Add(p);
            }
        }
    }
}
