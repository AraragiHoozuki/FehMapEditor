using FehMapEditor.Structs;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FehMapEditor.HSDArc;
using System.Windows.Media.Imaging;
using System;
using System.Windows.Media;
using System.Linq.Expressions;

namespace FehMapEditor
{
    public static class MasterData
    {
        public static Dictionary<string, string> Msgs;
        public static List<Skill> Skills;
        public static List<Person> Persons;
        public static List<uint> Versions;
        private static bool inited = false;

        private static BitmapSource[] ICON_ATLAS;
        private static BitmapSource STATUS;
        private static BitmapSource FACE_FRAME;
        private static BitmapSource FACE_FRAME_GREEN;
        private static BitmapSource FACE_FRAME_RED;
        private static Dictionary<string, BitmapSource> FACES = new();
        private static Dictionary<string, BitmapSource> FIELD_BACKS = new();

        private static readonly string DATAEXT = "*.lz";
        public static readonly string MSG_PATH = Directory.GetCurrentDirectory() + @"\Data\";
        public static readonly string SKL_PATH = Directory.GetCurrentDirectory() + @"\SRPG\Skill\";
        public static readonly string PERSON_PATH = Directory.GetCurrentDirectory() + @"\SRPG\Person\";
        public static readonly string ENEMY_PATH = Directory.GetCurrentDirectory() + @"\SRPG\Enemy\";
        public static readonly string FACE_PATH = Directory.GetCurrentDirectory() + @"\FACE\";
        public static readonly string FIELD_PATH = Directory.GetCurrentDirectory() + @"\Field\";
        public static readonly string UI_PATH = Directory.GetCurrentDirectory() + @"\UI\";

        [System.Runtime.InteropServices.DllImport("gdi32.dll")]
        public static extern bool DeleteObject(IntPtr hObject);
        private static BitmapSource Bitmap2BitmapImage(System.Drawing.Bitmap bitmap)
        {
            IntPtr hBitmap = bitmap.GetHbitmap();
            BitmapSource retval;
            try
            {
                retval = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                             hBitmap,
                             IntPtr.Zero,
                             System.Windows.Int32Rect.Empty,
                             BitmapSizeOptions.FromEmptyOptions());
            }
            finally
            {
                DeleteObject(hBitmap);
            }

            return retval;
        }
        public static BitmapSource LoadWebpImage(string path)
        {
            if (File.Exists(path))
            {
                byte[] bytes = File.ReadAllBytes(path);
                System.Drawing.Bitmap bitmap = Dynamicweb.WebP.Decoder.Decode(bytes);
                return Bitmap2BitmapImage(bitmap);
            } else
            {
                return BitmapSource.Create(1, 1, 1, 1, PixelFormats.BlackWhite, null, new byte[] { 0 }, 1);
            }
        }

        public static void Init()
        {
            string[] messages = Directory.GetFiles(MSG_PATH, DATAEXT);
            Msgs = new Dictionary<string, string>();
            foreach (string path in messages)
            {
                MsgArc ma = new MsgArc(path);
                Msgs = Msgs.Concat(ma.Items).ToDictionary(k => k.Key, v => v.Value);
            }

            string[] skills = Directory.GetFiles(SKL_PATH, DATAEXT);
            Skills = new List<Skill>();
            foreach (string path in skills)
            {
                SkillArc sf = new SkillArc(path);
                Skills.AddRange(sf.Items);
            }
            Skills = Skills.OrderBy(sk => sk.timestamp).ToList();

            string[] persons = Directory.GetFiles(PERSON_PATH, DATAEXT);
            Persons = new List<Person>();
            foreach (string path in persons)
            {
                PersonArc pf = new PersonArc(path);
                Persons.AddRange(pf.Items);
            }
            persons = Directory.GetFiles(ENEMY_PATH, DATAEXT);
            foreach (string path in persons)
            {
                EnemyArc e = new EnemyArc(path);
                Persons.AddRange(e.Items);
            }
            
            Versions = Persons.Select<Person, uint>(p => p.version_num).Distinct().ToList();
            Versions.Sort();

            InitImages();

            inited = true;
        }


        private static void InitImages()
        {
            ICON_ATLAS = new BitmapSource[11];
            for (int i = 0; i < 11; i++)
            {
                ICON_ATLAS[i] = LoadWebpImage(UI_PATH + "Skill_Passive" + (i + 1) + ".png");
            }
            STATUS = LoadWebpImage(UI_PATH + "Status.png");
            BitmapSource FACE_FRAMES = LoadWebpImage(UI_PATH + "MiniFace.png");
            FACE_FRAME = new CroppedBitmap(FACE_FRAMES, new System.Windows.Int32Rect(0, 0, 180, 180));
            FACE_FRAME_GREEN = new CroppedBitmap(FACE_FRAMES, new System.Windows.Int32Rect(180, 180, 180, 180));
            FACE_FRAME_RED = new CroppedBitmap(FACE_FRAMES, new System.Windows.Int32Rect(0, 360, 180, 180));
        }

        public static string GetMessage(string key)
        {
            if (inited && Msgs.ContainsKey(key))
            {
                return Msgs[key];
            }
            else
            {
                return key;
            }
        }


        public static BitmapSource GetFace(string key)
        {
            if (key == null)
            {
                return BitmapSource.Create(1, 1, 1, 1, PixelFormats.BlackWhite, null, new byte[] { 0 }, 1);
            } else if (FACES.ContainsKey(key))
            {
                return FACES[key];
            }
            else
            {
                BitmapSource b = LoadWebpImage($"{FACE_PATH}{key}\\Face_FC.png");
                //b = new CroppedBitmap(b, new System.Windows.Int32Rect(88, 0, 300, 300));
                //b = new TransformedBitmap(b, new ScaleTransform(0.25, 0.25));
                b = new TransformedBitmap(b, new ScaleTransform(0.4, 0.4));
                b.Freeze();
                FACES.Add(key, b);
                return b;
            }
        }

        public static BitmapSource GetFieldBack(string key)
        {
            if (key == null)
            {
                return BitmapSource.Create(1, 1, 1, 1, PixelFormats.BlackWhite, null, new byte[] { 0 }, 1);
            }
            else if (FIELD_BACKS.ContainsKey(key))
            {
                return FIELD_BACKS[key];
            }
            else
            {
                if (File.Exists($"{FIELD_PATH}{key}.png"))
                {
                    BitmapSource b = LoadWebpImage($"{FIELD_PATH}{key}.png");
                    FIELD_BACKS.Add(key, b);
                    return b;
                }
                else if (File.Exists($"{FIELD_PATH}{key}.jpg"))
                {
                    BitmapSource b = new BitmapImage(new Uri($"{FIELD_PATH}{key}.jpg"));
                    FIELD_BACKS.Add(key, b);
                    return b;
                }
                else
                {
                    return GetFieldBack(null);
                }
                
            }
        }

        public static BitmapSource GetIcon(int id)
        {
            int pic = id / 169;
            if (pic > ICON_ATLAS.Length) { pic = 0; id = 1; }
            int pos = id % 169;
            int row = pos / 13;
            int col = pos - row * 13;
            return new CroppedBitmap(ICON_ATLAS[pic], new System.Windows.Int32Rect(col * 76, row * 76, 76, 76));
        }

        public static BitmapSource GetWeaponIcon(int id)
        {
            return new CroppedBitmap(STATUS, new System.Windows.Int32Rect(56 * id, 59, 56, 56));
        }

        public static BitmapSource GetMoveIcon(int id)
        {
            CroppedBitmap cm =  new CroppedBitmap(STATUS, new System.Windows.Int32Rect(353 + 55 * id, 267, 55, 55));
            //TransformedBitmap tbm = new TransformedBitmap(cm, new RotateTransform(-90));
            return cm;
        }

        public static Skill FindSkill(string id)
        {
            return Skills.Find(sk => sk.id == id);
        }

        public static BitmapSource FaceFrame => FACE_FRAME;
        public static BitmapSource FaceFrameGreen => FACE_FRAME_GREEN;
        public static BitmapSource FaceFrameRed => FACE_FRAME_RED;
    }
}
