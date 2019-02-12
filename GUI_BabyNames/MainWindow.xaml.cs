using System;
using System.Collections.Generic;
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
        private string[,] topNames = new string[11,11];

        public MainWindow()
        {
            InitializeComponent();
        }

        private void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            babyNames = new List<BabyName>();
            try
            {   // Open the text file using a stream reader.
                using (StreamReader sr = new StreamReader("../../BabyNames.txt"))
                {
                    // Read the stream to a string, and write the string to the console.
                    while (!sr.EndOfStream)
                    {
                        String line = sr.ReadLine();
                        var babyNameToAdd = new BabyName(line);
                        babyNames.Add(babyNameToAdd);
                        for (int year = 0; year < 11; year++)
                        {
                            var rank = babyNameToAdd.Rank(year*10 + 1900);
                            if (rank < 10)
                            {
                                topNames[year,rank] = babyNameToAdd.Name;
                            }
                        }
                    }
                }
            }
            catch (IOException exception)
            {
                MessageBox.Show($"The file could not be read: {exception.Message}");
            }
        }

        private void DecadeLB_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var listboxSender = (ListBox)sender;
            List<string> stringList = new List<string>();
            for (int i = 0; i < 11; i++)
            {
                stringList.Add(topNames[listboxSender.SelectedIndex,i]);
            }

            listboxSender.ItemsSource = stringList;
        }
    }
}
