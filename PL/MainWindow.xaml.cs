using System.Windows;
using BlApi;


namespace PL
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        BlApi.Ibl IblObj = BlApi.BlFactory.GetBl();
        public MainWindow()
        {
            InitializeComponent();
        }


        private void ShowDronesButton_Click(object sender, RoutedEventArgs e) // shows drone list button
        {
            new DroneListWindow(IblObj).Show();
        }
    }
}
