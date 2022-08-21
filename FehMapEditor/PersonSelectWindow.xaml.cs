using FehMapEditor.Structs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace FehMapEditor
{
    /// <summary>
    /// PersonSelectWindow.xaml 的交互逻辑
    /// </summary>
    public partial class PersonSelectWindow : Window
    {
        public PersonSelectWindow()
        {
            InitializeComponent();
            WeaponTypeFilter.ItemsSource = Enum.GetNames(typeof(WeaponType));
            MoveTypeFilter.ItemsSource = Enum.GetNames(typeof(MoveType));
            VersionFilter.ItemsSource = MasterData.Versions;
        }

        public Person SelectedPerson { get; set; }

        private void PersonSelected(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            SelectedPerson = btn.DataContext as Person;
            DialogResult = true;
        }
        private void FilterPersons(object sender, RoutedEventArgs e)
        {
            PersonSelectControl.ItemsSource = MasterData.Persons.Where(p => FilterMoveType(p) && FilterWeaponType(p) && FilterVersion(p));
        }

        private bool FilterWeaponType(Person p)
        {
            return WeaponTypeFilter.SelectedIndex == -1 || p.weapon_type == (WeaponType)WeaponTypeFilter.SelectedIndex;
        }
        private bool FilterMoveType(Person p)
        {
            return MoveTypeFilter.SelectedIndex == -1 || p.move_type == (MoveType)MoveTypeFilter.SelectedIndex;
        }

        private bool FilterVersion(Person p)
        {
            return VersionFilter.SelectedIndex == -1 || p.version_num == (uint)VersionFilter.SelectedItem;
        }

    }
}
