using FehMapEditor.Structs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FehMapEditor.HSDArc
{
    public class PersonArc : FEHArchive
    {
        protected static readonly ulong ListKey = 0xDE51AB793C3AB9E1;

        public List<Person> Items { get; } = new List<Person>();
        public PersonArc(string path) : base(path) { }

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
                Person p = new Person
                {
                    id = ReadCryptedString(),
                    roman = ReadCryptedString(),
                    face = ReadCryptedString(),
                    face2 = ReadCryptedString(),
                    legendary = stream.ReadBytes(16),
                    timestamp = ReadXored64(0xBDC1E742E9B6489B),
                    id_num = ReadXored32(0x5F6E4E18),
                    version_num = ReadXored32(0x2E193A3C),
                    sort_value = ReadXored32(0x2A80349B),
                    origins = ReadXored32(0xE664B808),
                    weapon_type = (WeaponType)ReadXored8(0x06),
                    tome_class = (Element)ReadXored8(0x35),
                    move_type = (MoveType)ReadXored8(0x2A),
                    series = ReadXored8(0x43),
                    regular = ReadXored8(0xA1),
                    permanent = ReadXored8(0xC7),
                    base_vector = ReadXored8(0x3D),
                    is_refresher = ReadXored8(0xFF),
                    dragonflowers = ReadXored8(0xE4),
                    unknown = stream.ReadBytes(7),
                    stats = ReadStats(),
                    grow = ReadStats(),
                    skills = ReadSkills()
            };
                Items.Add(p);
            }
        }

        private string[][] ReadSkills()
        {
            string[][] skills = new string[5][];
            for (int i = 0; i < 5; i++)
            {
                skills[i] = new string[14];
                for (int j = 0; j < 14; j++)
                {
                    skills[i][j] = ReadCryptedString();
                }
            }
            return skills;
        }        
    }

    public class EnemyArc : FEHArchive
    {
        protected static readonly ulong ListKey = 0x62CA95119CC5345C;

        public List<Enemy> Items { get; } = new List<Enemy>();
        public EnemyArc(string path) : base(path) { }

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
                Enemy e = new Enemy
                {
                    id = ReadCryptedString(),
                    roman = ReadCryptedString(),
                    face = ReadCryptedString(),
                    face2 = ReadCryptedString(),
                    top_weapon = ReadCryptedString(),
                    assist1 = ReadCryptedString(),
                    assist2 = ReadCryptedString(),
                    unknown_e1 = ReadCryptedString(),
                    timestamp = ReadXored64(0xBDC1E742E9B6489B),
                    id_num = ReadXored32(0x422F41D4),
                    weapon_type = (WeaponType)ReadXored8(0xE4),
                    tome_class = (Element)ReadXored8(0x81),
                    move_type = (MoveType)ReadXored8(0x0D),
                    random_allowed = ReadXored8(0xC4),
                    is_boss = ReadXored8(0x6A),
                    is_refresher = ReadXored8(0x2A),
                    is_enemy = ReadXored8(0x13),
                    padding = stream.ReadBytes(5),
                    stats = ReadStats(),
                    grow = ReadStats(),
                };
                Items.Add(e);
            }
        }
    }
}
