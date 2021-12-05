using System.Windows; 
using PidControllerWpf.Commands; 
using PidControllerWpf.Views; 

namespace PidControllerWpf.ViewModels
{
    public class MainWindowVM
    {
        private MainWindow MainWindow {get; set; }

        public RedirectCommand RedirectCommand { get; private set; }

        private PidVM pidVM;
        public PidVM PidVM
        {
            get { return pidVM; }
        }

        private TextBlockVM textBlockVM;
        public TextBlockVM TextBlockVM
        {
            get { return textBlockVM; }
        }

        private GraphCanvasVM graphCanvasVM;
        public GraphCanvasVM GraphCanvasVM
        {
            get { return graphCanvasVM; }
        }

        public MainWindowVM(MainWindow mainWindow)
        {
            this.MainWindow = mainWindow; 

            this.textBlockVM = new TextBlockVM(); 
            this.graphCanvasVM = new GraphCanvasVM();
            this.pidVM = new PidVM(ref textBlockVM, ref graphCanvasVM); 

            this.RedirectCommand = new RedirectCommand(this); 
        }

        public void OpenGraph2D()
        {
            HideAllPages(); 
            this.MainWindow.Graph2D.Visibility = Visibility.Visible; 
        }

        public void OpenBarCharts()
        {
            HideAllPages(); 
            this.MainWindow.BarCharts.Visibility = Visibility.Visible; 
        }

        private void HideAllPages()
        {
            this.MainWindow.Graph2D.Visibility = Visibility.Hidden; 
            this.MainWindow.BarCharts.Visibility = Visibility.Hidden; 
        }
    }
}