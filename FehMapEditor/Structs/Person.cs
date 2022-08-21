using System.Windows.Media.Imaging;

namespace FehMapEditor.Structs
{
    public class Person
    {
        public string id;
        public string roman;
        public string face;
        public string face2;
        public byte[] legendary;//16 bytes Legendary info
        public ulong timestamp;
        public uint id_num;
        public uint version_num = 65535;
        public uint sort_value;
        public uint origins;
        public WeaponType weapon_type;
        public Element tome_class;
        public MoveType move_type;
        public byte series;
        public byte regular;
        public byte permanent;
        public byte base_vector;
        public byte is_refresher;
        public byte dragonflowers;
        public byte[] unknown; //7bytes offset
        public Stats stats;
        public Stats grow;
        public string[][] skills;

        public string Name => MasterData.GetMessage("M" + id);
        public BitmapSource FaceImage => MasterData.GetFace(face);
        public BitmapSource FaceFrame => MasterData.FaceFrame;

        public BitmapSource WeaponIcon => MasterData.GetWeaponIcon((int)weapon_type);
        public BitmapSource MoveIcon => MasterData.GetMoveIcon((int)move_type);

        public int Stat(int index, int hone = 0)
        {
            int value = grow[index] + 5 * hone;
            value = value * 114 / 100;
            value = value * 39 / 100;
            value = value + stats[index] + 1 + hone;
            return value;
        }
    }

    public class Enemy : Person
    {
        public string top_weapon;
        public string assist1;
        public string assist2;
        public string unknown_e1;
        public byte random_allowed;
        public byte is_boss;
        public byte is_enemy;
        public byte[] padding; //5
    }
}
