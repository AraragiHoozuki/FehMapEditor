using FehMapEditor.HSDArc;
using FehMapEditor.Structs;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace FehMapEditor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        MapArc mapfile;
        SRPGMap Map { get => mapfile.Map; }

        MapGridInfo[][] grids;
        MapGridInfo current_grid;
        Unit cloned_unit;

        PersonSelectWindow psw;
        SkillSelectWindow ssw;
        public MainWindow()
        {
            InitializeComponent();
            TerrainTypeCombo.ItemsSource = Enum.GetNames(typeof(TerrainType));
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog()
            {
                Filter = "*.bin|*.bin.lz"
            };
            bool? result = dialog.ShowDialog();
            if (result == true)
            {
                mapfile = new MapArc(dialog.FileName);
                OnMapLoad();
            }
        }

        private void OnMapLoad()
        {
            grids = new MapGridInfo[Map.field.terrain.Length][];
            for(int i = 0; i < Map.field.terrain.Length; i++)
            {
                int view_y = Map.field.terrain.Length - i - 1;
                grids[view_y] = new MapGridInfo[Map.field.terrain[0].Length];
                for (int j = 0; j < Map.field.terrain[0].Length; j++)
                {
                    grids[view_y][j] = new MapGridInfo()
                    {
                        terrain = Map.field.terrain[i][j],
                        y = (ushort)i,
                        x = (ushort)j,
                        unit = Array.Find(Map.units, (Unit u) => u.pos.x == j && u.pos.y == i)
                    };
                }
            }
            MapGrid.ItemsSource = grids;
            MapGrid.DataContext = Map;
            DetailPanel.DataContext = Map;
        }

        private void GridBtnClick(object sender, RoutedEventArgs e)
        {
            Image btn = sender as Image;
            current_grid = btn.DataContext as MapGridInfo;
            GridInfoPanel.DataContext = current_grid;
            UnitInfoPanel.DataContext = current_grid.unit;
        }

        private void IdBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            psw = new PersonSelectWindow();
            psw.PersonSelectControl.ItemsSource = MasterData.Persons;
            
            if (psw.ShowDialog() == true) {
                if (current_grid.unit == null)
                {
                    current_grid.unit = Unit.Create();
                }
                current_grid.unit.pos = new Position()
                {
                    x = current_grid.x,
                    y = current_grid.y
                };
                current_grid.unit.Id = psw.SelectedPerson.id;
                current_grid.unit.Hp = (ushort)(psw.SelectedPerson.Stat(0) + 10);
                current_grid.unit.Atk = (ushort)(psw.SelectedPerson.Stat(1) + 4);
                current_grid.unit.Spd = (ushort)(psw.SelectedPerson.Stat(2) + 4);
                current_grid.unit.Def = (ushort)(psw.SelectedPerson.Stat(3) + 4);
                current_grid.unit.Res = (ushort)(psw.SelectedPerson.Stat(4) + 4);
                current_grid.unit.Weapon = psw.SelectedPerson.skills[4][0];
                List<string> skills = psw.SelectedPerson.skills[3].Concat(psw.SelectedPerson.skills[4]).Reverse().ToList();
                current_grid.unit.Assist = skills.Find(id => id != null && MasterData.FindSkill(id).category == SkillCategory.Assist);
                current_grid.unit.Special = skills.Find(id => id != null && MasterData.FindSkill(id).category == SkillCategory.Special);
                current_grid.unit.ASkill = skills.Find(id => id != null && MasterData.FindSkill(id).category == SkillCategory.A);
                current_grid.unit.BSkill = skills.Find(id => id != null && MasterData.FindSkill(id).category == SkillCategory.B);
                current_grid.unit.CSkill = skills.Find(id => id != null && MasterData.FindSkill(id).category == SkillCategory.C);
                current_grid.OnPropertyChanged("FaceImage");
            }
        }

        private void SkillSlotDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ssw = new SkillSelectWindow();
            ssw.SkillList.ItemsSource = MasterData.Skills;
            int index = int.Parse((string)((TextBox)sender).Tag);
            if (current_grid != null && current_grid.unit != null)
            {
                Skill s = MasterData.FindSkill(current_grid.unit.skills[index]);
                if (s != null)
                {
                    ssw.SkillSearchField.Text = s.Name;
                }
            }
            if (ssw.ShowDialog() == true)
            {
                switch(index)
                {
                    case 0:
                        current_grid.unit.Weapon = ssw.SelectedSkill.id;
                        break;
                    case 1:
                        current_grid.unit.Assist = ssw.SelectedSkill.id;
                        break;
                    case 2:
                        current_grid.unit.Special = ssw.SelectedSkill.id;
                        break;
                    case 3:
                        current_grid.unit.ASkill = ssw.SelectedSkill.id;
                        break;
                    case 4:
                        current_grid.unit.BSkill = ssw.SelectedSkill.id;
                        break;
                    case 5:
                        current_grid.unit.CSkill = ssw.SelectedSkill.id;
                        break;
                    case 6:
                        current_grid.unit.SSkill = ssw.SelectedSkill.id;
                        break;
                    default:
                        break;
                }
            }
        }

        private void SaveMapBtn_Click(object sender, RoutedEventArgs e)
        {
            List<Unit> units = new List<Unit>();
            for (int i = 0; i < Map.field.terrain.Length; i++)
            {
                for (int j = 0; j < Map.field.terrain[0].Length; j++)
                {
                    Map.field.terrain[i][j] = grids[Map.field.terrain.Length - i - 1][j].terrain;
                    if (grids[Map.field.terrain.Length - i - 1][j].unit != null)
                    {
                        units.Add(grids[Map.field.terrain.Length - i - 1][j].unit);
                    }
                }
            }
            Map.units = units.ToArray();

            byte[] bin = SRPGMapWriter.WriteBin(mapfile);
            FileStream lzfs = File.OpenRead(mapfile.path);
            byte[] start = new byte[4];
            lzfs.Read(start, 0, 4);
            lzfs.Close();
            byte[] result = Crypter.EncryptAndCompress(start.Concat(bin).ToArray());
            File.WriteAllBytes(mapfile.path, result);
        }

        private void IdBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (IdBox.Text == "")
            {
                current_grid.unit = null;
            }
        }

        private void CloneUnit(object sender, RoutedEventArgs e)
        {
            if (current_grid.unit != null)
            {
                cloned_unit = current_grid.unit.Clone();
            }
        }

        private void PasteUnit(object sender, RoutedEventArgs e)
        {
            if (cloned_unit != null)
            {
                current_grid.unit = cloned_unit;
                cloned_unit.pos.x = current_grid.x;
                cloned_unit.pos.y = current_grid.y;
                //cloned_unit = null;
                current_grid.OnPropertyChanged("FaceImage");
            }
        }
    }
}
