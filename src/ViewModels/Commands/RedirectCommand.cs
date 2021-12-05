using System.Windows; 
using System.Windows.Input; 
using PidControllerWpf.ViewModels; 

namespace PidControllerWpf.Commands
{
    public class RedirectCommand : ICommand
    {
        private MainWindowVM MainWindowVM; 

        public RedirectCommand(MainWindowVM mainWindowVM)
        {
            this.MainWindowVM = mainWindowVM; 
        }

        public event System.EventHandler CanExecuteChanged; 
        
        public bool CanExecute(object parameter)
        {
            return true; 
        }

        public void Execute(object parameter)
        {
            string parameterString = parameter as string; 
            if (parameterString == "Graph2D")
            {
                this.MainWindowVM.OpenGraph2D(); 
            }
            else if (parameterString == "BarCharts")
            {
                this.MainWindowVM.OpenBarCharts(); 
            }
            else 
            {
                System.Windows.MessageBox.Show($"Incorrect parameter: {parameterString}", "Error"); 
            }
        }
    }
}