using System.Windows; 
using System.Windows.Input; 
using PidControllerWpf.ViewModel; 

namespace PidControllerWpf.Commands
{
    class StopTimerCommand : ICommand
    {
        private PidVM PidVM; 

        public StopTimerCommand(PidVM pidVM)
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
                this.PidVM.TimerGraph.Stop(); 
                GraphCanvasVM gcvm = this.PidVM.GraphCanvasVM; 
                gcvm.IsTimerEnabled = false; 

                // Set reference point to be able to change SP while timer isn't enabled
                Point refpoint = new Point(gcvm.SetpointLeft, gcvm.SetpointTop + 2.5); 
                gcvm.SpRefPoint = refpoint; 
            }
            catch (System.Exception e)
            {
                System.Windows.MessageBox.Show(e.Message, "Exception");
            }
        }
    }
}