using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Media.Imaging;

namespace FehMapEditor.Structs
{
    public class MapGridInfo : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        public byte TileTerrain { get => terrain; set => terrain = value; }
        public byte terrain;
        public Unit unit;
        public ushort x;
        public ushort y;


        public string CoordinateInfo => $"({x}, {y})";

        public BitmapSource FaceImage
        {
            get
            {
                if (unit == null)
                {
                    return null;
                } else {
                    return MasterData.GetFace(MasterData.Persons.Find(p => p.id == unit.id_tag).face);
                }
            }
        }
        public BitmapSource FaceFrame {
            get
            {
                if (unit == null)
                {
                    return MasterData.FaceFrame;
                } else if (unit.is_enemy == 1)
                {
                    return MasterData.FaceFrameRed;
                } else
                {
                    return MasterData.FaceFrameGreen;
                }
            }
        }
    }
}
