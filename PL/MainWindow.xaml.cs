using System.Windows;
using BlApi;
using BO;


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
            //rec rec = new("me");
            //MessageBox.Show(rec.hudis);
            //rec.hudis = "hi";
        }


        private void ShowDronesButton_Click(object sender, RoutedEventArgs e) // shows drone list button
        {
            new ListWindow(IblObj).Show();
        }
    }
}
