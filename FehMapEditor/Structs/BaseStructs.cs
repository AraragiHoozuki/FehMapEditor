namespace FehMapEditor.Structs
{
    public enum WeaponType
    {
        Sword, Lance, Axe, RedBow, BlueBow, GreenBow, ColorlessBow, RedDagger, BlueDagger,
        GreenDagger, ColorlessDagger, RedTome, BlueTome, GreenTome, ColorlessTome, Staff, RedBreath, BlueBreath, GreenBreath,
        ColorlessBreath, RedBeast, BlueBeast, GreenBeast, ColorlessBeast
    };
    public enum Element { None, Fire, Thunder, Wind, Light, Dark };
    public enum MoveType { Infantry, Armored, Cavalry, Flying };
    public enum TerrainType
    {
        Outdoor,
        Indoor,
        Desert,
        Forest,
        Mountain,
        River,
        Sea,
        Lava,
        Wall,
        OutdoorBreakable,
        OutdoorBreakable2,
        IndoorBreakable,
        IndoorBreakable2,
        DesertBreakable,
        DesertBreakable2,
        Bridge,
        OutdoorDefensive,
        ForestDefensive,
        IndoorDefensive,
        BridgeBreakable,
        BridgeBreakable2,
        Inaccessible,
        OutdoorTrench,
        IndoorTrench,
        OutdoorDefensiveTrench,
        IutdoorDefensiveTrench,
        IndoorWater,
        PlayerFortress,
        EnemyFortress,
        PlayerCamp,
        EnemyCamp,
        OutdoorPlayerCamp,
        IndoorPlayerCamp,
        PlayerStructure,
        EnemyStructure
    }
    public class Stats
    {
        public ushort hp;
        public ushort atk;
        public ushort spd;
        public ushort def;
        public ushort res;

        public int Total => hp + atk + spd + def + res;

        public int this[int index]
        {
            get
            {
                int value = index switch
                {
                    0 => hp,
                    1 => atk,
                    2 => spd,
                    3 => def,
                    4 => res,
                    _ => 0,
                };
                return value;
            }
        }
    }

    public class LegendaryInfo
    {
        public enum Element
        {
            None,
            Fire,
            Water,
            Wind,
            Earth,
            Light,
            Dark,
            Astra,
            Anima
        };
        public string duo_skill_id;
        public Stats bonus_effect;
        public byte kind; //0x21
        public Element element; //0x05
        public byte bst; //0x0f
        public byte pair_up; //0x80
    }

    public enum SkillCategory
    {
        Weapon,
        Assist,
        Special,
        A,
        B,
        C,
        S,
        Refine,
        Transform
    }
}
