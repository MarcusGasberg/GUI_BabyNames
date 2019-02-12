using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
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

namespace GUI_BabyNames
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<BabyName> babyNames;
        private string[,] topNames = new string[11,10];
        ObservableCollection<string> stringList = new ObservableCollection<string>();

        public MainWindow()
        {
            InitializeComponent();
            TopNameLB.ItemsSource = stringList;
        }

        private void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            babyNames = ReadBabyNames("../../BabyNames.txt");
            foreach (var babyName in babyNames)
            {
                for (int year = 0; year < 10; ++year)
                {
                    var rank = babyName.Rank(year*10 + 1900);

                    if (rank < 1 || rank > 10)
                        continue;

                    if (topNames[year, rank - 1] == null)
                    {
                        topNames[year, rank - 1] = babyName.Name;
                    }
                    else
                    {
                        topNames[year, rank - 1] += $" and {babyName.Name}";
                    }
                }
            }
        }

        private void DecadeLB_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var listboxSender = sender as ListBox;
            if(listboxSender == null)
                return;
            stringList.Clear();
            int decade = listboxSender.SelectedIndex;
            for (int i = 0; i < 10; i++)
            {
                stringList.Add($"#{i+1}: {topNames[decade, i]}");
            }

        }

        private List<BabyName> ReadBabyNames(string path)
        {
            if (!File.Exists(path))
                throw new FileNotFoundException();
            var babyNames = new List<BabyName>();
            try
            {
                using (StreamReader sr = new StreamReader(path))
                {
                    while (!sr.EndOfStream)
                    {
                        String line = sr.ReadLine();
                        var babyNameToAdd = new BabyName(line);
                        babyNames.Add(babyNameToAdd);
                    }
                }
            }
            catch (IOException exception)
            {
                MessageBox.Show($"The file could not be read: {exception.Message}");
            }

            return babyNames;
        }
    }
}
