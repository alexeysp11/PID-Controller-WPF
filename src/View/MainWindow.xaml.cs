using System.Windows;
using PidControllerWpf.ViewModel;
using PidControllerWpf.UserControls;

namespace PidControllerWpf.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            Loaded += (o, e) => 
            {
                MainWindowVM mainWindowVM = (MainWindowVM)(this.DataContext); 

                Configuration.DataContext = mainWindowVM; 
                Graph2D.DataContext = mainWindowVM; 
            }; 
        }
    }
}
