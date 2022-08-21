using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace FehMapEditor.Structs
{
    public class Skill
    {
        public string id;
        public string refine_base;
        public string name;
        public string description;
        public string refine_id;
        public string beast_effect_id;
        public string[] requirements; //2-length
        public string next_skill;
        public string[] sprites;// 4-length
        public Stats stats;
        public Stats class_params;
        public Stats combat_buffs;
        public Stats skill_params;
        public Stats skill_params2;
        public Stats refine_stats;
        public uint id_num;
        public uint sort_value;
        public uint icon;
        public uint wep_equip;
        public uint mov_equip;
        public uint sp_cost;
        public SkillCategory category;
        public Element tome_class;
        public byte is_exclusive;
        public byte enemy_only;
        public byte range;
        public byte might;
        public byte cooldown;
        public byte assist_cd;
        public byte healing;
        public byte skill_range;
        public ushort score;
        public byte promotion_tier;
        public byte promotion_rarity;
        public byte is_refined;
        public byte refine_sort_id;
        public uint tokkou_wep;
        public uint tokkou_mov;
        public uint shield_wep;
        public uint shield_mov;
        public uint weak_wep;
        public uint weak_mov;
        public uint unknown1;
        public uint unknown2;
        public uint adaptive_wep;
        public uint adaptive_mov;
        public uint timing;
        public uint ability;
        public uint limit1;
        public ushort[] limit1_params;//2-length
        public uint limit2;
        public ushort[] limit2_params;//2-length
        public uint target_wep;
        public uint target_mov;
        public string passive_next;
        public ulong timestamp;
        public byte random_allowed;
        public byte min_lv;
        public byte max_lv;
        public byte tt_inherit_base;
        public byte random_mode;
        public byte[] padding1; //3-length
        public uint limit3;
        public ushort[] limit3_params;//2-length
        public byte range_shape;
        public byte target_either;
        public byte distant_counter;
        public byte canto;
        public byte pathfinder;
        public byte[] padding2; //3-length

        public string Name => MasterData.GetMessage(name);
        public string Description { 
            get
            {
                string s = "";
                if (this.category == SkillCategory.Weapon)
                {
                    s += $"攻击: {might}\n";
                }
                s += MasterData.GetMessage(description);
                if (refine_id != null)
                {
                    s += MasterData.GetMessage(MasterData.Skills.Find(sk => sk.id == refine_id).description);
                }
                return s;
            }
        }
        

        public BitmapSource Icon => MasterData.GetIcon((int)icon);
    }
}
