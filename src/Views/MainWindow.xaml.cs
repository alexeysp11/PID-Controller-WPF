using System.Windows;
using PidControllerWpf.ViewModels;
using PidControllerWpf.UserControls;

namespace PidControllerWpf.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MainWindowVM MainWindowVM { get; set; }

        public MainWindow()
        {
            InitializeComponent();

            Loaded += (o, e) => 
            {
                this.MainWindowVM = new MainWindowVM(this); 

                this.DataContext = this.MainWindowVM; 
                Menu.DataContext = this.MainWindowVM; 
                Configuration.DataContext = this.MainWindowVM; 
                Graph2D.DataContext = this.MainWindowVM; 
            }; 
        }
    }
}
