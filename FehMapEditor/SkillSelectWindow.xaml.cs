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
    /// SkillSelectWindow.xaml 的交互逻辑
    /// </summary>
    public partial class SkillSelectWindow : Window
    {
        public Skill SelectedSkill { get; set; }
        public SkillSelectWindow()
        {
            InitializeComponent();
            WeaponTypeFilter.ItemsSource = Enum.GetNames(typeof(WeaponType));
            MoveTypeFilter.ItemsSource = Enum.GetNames(typeof(MoveType));
        }

        private void FilterChanged(object sender, RoutedEventArgs e)
        {
            if(SkillList != null)
            {
                SkillList.ItemsSource = MasterData.Skills.Where(sk => FilterText(sk) && FilterCategory(sk) && FilterWeaponType(sk) && FilterMoveType(sk) && FilterExclusive(sk) && FilterSp(sk) && FilterRefined(sk) && FilterCanto(sk));
            }
        }

        private bool FilterText(Skill s)
        {
            if (SkillSearchField.Text == "")
            {
                return true;
            } else
            {
                return s.Name.Contains(SkillSearchField.Text);
            }
        }
        private bool FilterCategory(Skill s)
        {
            if (AssistChecker.IsChecked != true && SpecialChecker.IsChecked != true && AChecker.IsChecked != true && BChecker.IsChecked != true && CChecker.IsChecked != true)
            {
                return true;
            } else
            {
                return 
                    (AssistChecker.IsChecked == true && s.category == SkillCategory.Assist) ||
                    (SpecialChecker.IsChecked == true && s.category == SkillCategory.Special) ||
                    (AChecker.IsChecked == true && s.category == SkillCategory.A) ||
                    (BChecker.IsChecked == true && s.category == SkillCategory.B) ||
                    (CChecker.IsChecked == true && s.category == SkillCategory.C);
            }
        }

        private bool FilterWeaponType(Skill s)
        {
            if (WeaponTypeFilter.SelectedIndex == -1)
            {
                return true;
            } else if (s.category != SkillCategory.Weapon)
            {
                return false;
            } else
            {
                return ((s.wep_equip >> WeaponTypeFilter.SelectedIndex) & 1) == 1;
            }
        }

        private bool FilterMoveType(Skill s)
        {
            if (MoveTypeFilter.SelectedIndex == -1)
            {
                return true;
            }
            else
            {
                return ((s.mov_equip >> MoveTypeFilter.SelectedIndex) & 1) == 1;
            }
        }

        private bool FilterExclusive(Skill s)
        {
            if (ExclusiveChecker.IsChecked == true && s.is_exclusive == 0)
            {
                return false;
            } else if (s.category == SkillCategory.Weapon && s.refine_base != null && MasterData.Skills.Find(sk => sk.id == s.refine_base).is_exclusive == 0)
            {
                return false;
            } else { 
                return true;
            }
        }

        private bool FilterSp(Skill s)
        {
            return s.sp_cost >= SpSlider.Value;
        }

        private bool FilterRefined(Skill s)
        {
            return !(RefineChecker.IsChecked == true && (s.is_refined == 0||s.refine_id == null));
        }

        private bool FilterCanto(Skill s)
        {
            return !(CantoChecker.IsChecked == true && s.canto == 0);
        }

        private void SkillList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            SelectedSkill = SkillList.SelectedItem as Skill;
            if (SelectedSkill != null)
            {
                DialogResult = true;
            }
        }
    }
}
