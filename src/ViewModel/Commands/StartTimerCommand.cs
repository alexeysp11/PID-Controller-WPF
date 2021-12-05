using System.Windows.Input; 
using PidControllerWpf.ViewModel; 

namespace PidControllerWpf.Commands
{
    public class StartTimerCommand : ICommand 
    {
        private PidVM PidVM { get; set; } 

        public StartTimerCommand(PidVM pidVM)
        {
            this.PidVM = pidVM; 
        }

        public event System.EventHandler CanExecuteChanged; 
        
        public bool CanExecute(object parameter)
        {
            return true; 
        }

        public void Execute(object parameter)
        {
            try
            {
                this.PidVM.TimerGraph.Start(); 
                this.PidVM.GraphCanvasVM.IsTimerEnabled = true; 
            }
            catch (System.Exception e)
            {
                System.Windows.MessageBox.Show(e.Message, "Exception");
            }
        }
    }
}