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
using System.Windows.Navigation;
using System.Windows.Shapes;


namespace BabynamesUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        [System.ComponentModel.Bindable(true)]
        [System.ComponentModel.TypeConverter(typeof(System.Windows.FontSizeConverter))]
        [System.Windows.Localizability(System.Windows.LocalizationCategory.None)]
        public double FontSize { get; set; }

        private int ranksToShow = 10;
        private int _Decades = 11;
        private List<BabyName> _BabyList;
        private string[,] RankingArray;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void lstDecadeTopNames_Loaded(object sender, RoutedEventArgs e)
        {

            for (int i = 0; i < ranksToShow; i++)
            {
                lstDecadeTopNames.Items.Add(RankingArray[0,i]);
            } 
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            _BabyList = new List<BabyName>();
            RankingArray = new string[11, ranksToShow];
            var lines = System.IO.File.ReadLines(@"babynames.txt");
            foreach (var line in lines)
            {
                var toAdd = new BabyName(line);
                _BabyList.Add(toAdd);
                for (int i = 0; i < 11; i++)
                {
                    int rank = toAdd.Rank((i + 190) * 10);
                    if (rank > 0 & rank < 11)
                    {
                        if (RankingArray[i, rank - 1] == null)
                            RankingArray[i, rank - 1] = rank + ". " + toAdd.Name;
                        else
                            RankingArray[i, rank - 1] += " and " + toAdd.Name;
                    }

                }

            }
        }

        private void ComboBox_Loaded(object sender, RoutedEventArgs e)
        {
            for (int i = 190; i < 201; i++)
            {
                comboDecadeYears.Items.Add(i * 10);
            }
        }



        private void comboDecadeYears_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int year = comboDecadeYears.SelectedIndex;
            lstDecadeTopNames.Items.Clear();
            for (int i = 0; i < ranksToShow; i++)
            {
                lstDecadeTopNames.Items.Add(RankingArray[year, i]);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var nameToSearch = tbxName.Text;
            BabyName target=null;
            while(target==null)
            {
                foreach (var baby in _BabyList)
                {
                    if (baby.Name == nameToSearch)
                        target = baby;
                }
                break;
            }

            //exception for baby not present
            if(target==null)
            {
                MessageBox.Show("Sorry, the name is not present in the database");
                return;
            }
            //load baby stats
            listboxSrchRes.Items.Clear();
            tblAvgRank.Text = target.AverageRank().ToString();
            tblTrend.Text = target.Trend().ToString();
            for (int i = 0; i < _Decades; i++)
            {
                int yearToSearch = (i + 190) * 10;
                string item = new string($"{yearToSearch} {target.Rank(yearToSearch)}");
                listboxSrchRes.Items.Add(item);
            }
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.FontSize = 8;
        }

        private void MenuFontNormal_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.FontSize = 16;
        }

        private void MenuFontLarge_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.FontSize = 24;
        }

        private void MenuFontHuge_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.FontSize = 32;
        }

        private void MenuExit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.Close();
        }
    }
}
