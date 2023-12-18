using System.Windows;
using WpfApp4.Model;
using WpfApp4.StartUpHelper;
using WpfApp4.View;

namespace WpfApp4
{

    public partial class MainWindow : Window
    {
        private readonly IAbsrtactFactory<GridDB> factory;

        public MainWindow(IAbsrtactFactory<GridDB> factory)
        {

            InitializeComponent();
            this.factory = factory;
            DataContext = new MainView(factory);
        }
    }
}