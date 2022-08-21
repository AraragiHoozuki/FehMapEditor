using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace FehMapEditor.Structs
{
    public class SRPGMap
    {
        public uint unknown;
        public uint highest_score;
        public ulong addr_field;
        public ulong addr_player_pos;
        public ulong addr_units;
        public uint player_count;
        public uint unit_count;
        public byte turns_to_win;
        public byte last_enemy_phase;
        public byte turns_to_defend;
        public byte[] padding;
        public Field field;
        public Position[] player_pos;
        public Unit[] units;
        public string FieldId { get => field.id; set { field.id = value; } }

        public BitmapSource FieldBG => MasterData.GetFieldBack(field.id);
    }

    public class Field
    {
        public string id;
        public uint width;
        public uint height;
        public byte base_terrain;
        public byte[] padding;
        public byte[][] terrain;//from left bottom

    }

    public class Position
    {
        public ushort x;
        public ushort y;
    }

    public class Unit: INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public static Unit Create()
        {
            return new Unit()
            {
                skills = new string[7],
                accessory = null,
                rarity = 5,
                lv = 40,
                cd = 255,
                stats = new Stats(),
                start_turn = 1,
                movement_group = 255,
                movement_delay = 255,
                break_terrain = 0,
                tether = 0,
                true_lv = 50,
                is_enemy = 0,
                padding = 0,
                spawn_check = null,
                spawn_count = 255,
                spawn_turns = 255,
                spawn_target_remain = 255,
                spawn_target_kills = 255,
                paddings = new byte[4]
            };
        }

        public string id_tag;
        public string[] skills;
        public string accessory;
        public Position pos;
        public byte rarity;
        public byte lv;
        public byte cd;
        public byte unknown = 100;
        public Stats stats;
        public byte start_turn;
        public byte movement_group;
        public byte movement_delay;
        public byte break_terrain;
        public byte tether;
        public byte true_lv;
        public byte is_enemy;
        public byte padding;
        public string spawn_check;
        public byte spawn_count;
        public byte spawn_turns;
        public byte spawn_target_remain;
        public byte spawn_target_kills;
        public byte[] paddings;

        public Unit Clone()
        {
            return new Unit()
            {
                id_tag = id_tag,
                skills = skills.Clone() as string[],
                accessory = accessory,
                pos = new Position() { x = pos.x, y = pos.y },
                rarity = rarity,
                lv = lv,
                cd = cd,
                unknown = unknown,
                stats = new Stats() { atk = stats.atk, def = stats.def, res = stats.res, spd = stats.spd, hp = stats.hp },
                start_turn = start_turn,
                movement_group = movement_group,
                movement_delay = movement_delay,
                break_terrain = break_terrain,
                tether = tether,
                true_lv = true_lv,
                is_enemy = is_enemy,
                padding = padding,
                spawn_check = spawn_check,
                spawn_count = spawn_count,
                spawn_turns = spawn_turns,
                spawn_target_remain = spawn_target_remain,
                spawn_target_kills = spawn_target_kills,
                paddings = paddings.Clone() as byte[]
            };
        }

        public string Id { get => id_tag; set { id_tag = value; OnPropertyChanged(); OnPropertyChanged("Name"); } }
        public string Name { get => MasterData.GetMessage("M" + id_tag); }
        public ushort Hp { get => stats.hp; set { stats.hp = value; OnPropertyChanged(); OnPropertyChanged("TotalStats"); } }
        public ushort Atk { get => stats.atk; set { stats.atk = value; OnPropertyChanged(); OnPropertyChanged("TotalStats"); } }
        public ushort Spd { get => stats.spd; set { stats.spd = value; OnPropertyChanged(); OnPropertyChanged("TotalStats"); } }
        public ushort Def { get => stats.def; set { stats.def = value; OnPropertyChanged(); OnPropertyChanged("TotalStats"); } }
        public ushort Res { get => stats.res; set { stats.res = value; OnPropertyChanged(); OnPropertyChanged("TotalStats"); } }
        public int TotalStats => stats.Total;

        public byte Lv { get => lv; set => lv = value; }
        public byte TLv { get => true_lv; set => true_lv = value; }
        public byte SpecialCD { get => cd; set => cd = value; }
        public byte IsEnemy { get => is_enemy; set => is_enemy = value; }

        public string Weapon { get => skills[0]; set { skills[0] = value; OnPropertyChanged(); OnPropertyChanged("WeaponImage"); } }
        public BitmapSource WeaponImage => GetSkillImage(0);
        public string Assist { get => skills[1]; set { skills[1] = value; OnPropertyChanged(); OnPropertyChanged("AssistImage"); } }
        public BitmapSource AssistImage => GetSkillImage(1);
        public string Special { get => skills[2]; set { skills[2] = value; OnPropertyChanged(); OnPropertyChanged("SpecialImage"); } }
        public BitmapSource SpecialImage => GetSkillImage(2);
        public string ASkill { get => skills[3]; set { skills[3] = value; OnPropertyChanged(); OnPropertyChanged("ASkillImage"); } }
        public BitmapSource ASkillImage => GetSkillImage(3);
        public string BSkill { get => skills[4]; set { skills[4] = value; OnPropertyChanged(); OnPropertyChanged("BSkillImage"); } }
        public BitmapSource BSkillImage => GetSkillImage(4);
        public string CSkill { get => skills[5]; set { skills[5] = value; OnPropertyChanged(); OnPropertyChanged("CSkillImage"); } }
        public BitmapSource CSkillImage => GetSkillImage(5);
        public string SSkill { get => skills[6]; set { skills[6] = value; OnPropertyChanged(); OnPropertyChanged("SSkillImage"); } }
        public BitmapSource SSkillImage => GetSkillImage(6);

        private BitmapSource GetSkillImage(int index)
        {
            if (skills[index] == null)
            {
                return null;
            }
            else
            {
                Skill s = MasterData.Skills.Find(s => s.id == skills[index]);
                return s != null ? MasterData.GetIcon((int)MasterData.Skills.Find(s => s.id == skills[index]).icon) : null;
            }
        }
    }
}
