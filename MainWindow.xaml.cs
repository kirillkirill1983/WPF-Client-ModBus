using System.Windows;
using WpfApp4.Model;

namespace WpfApp4
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            DataContext = new MainView();
            InitializeComponent();
        }
    }
}