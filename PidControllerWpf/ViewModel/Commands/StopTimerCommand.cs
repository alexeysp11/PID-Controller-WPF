using System.Windows; 
using System.Windows.Input; 
using PidControllerWpf.ViewModel; 
using PidControllerWpf.Exceptions; 

namespace PidControllerWpf.Commands
{
    class StopTimerCommand : ICommand
    {
        #region Members
        private PidVM _PidVM; 
        #endregion  // Members

        #region Constructor
        public StopTimerCommand(PidVM PidVM)
        {
            this._PidVM = PidVM; 
        }
        #endregion  // Constructor

        public event System.EventHandler CanExecuteChanged; 
        
        public bool CanExecute(object parameter)
        {
            return true; 
        }

        public void Execute(object parameter)
        {
            try
            {
                // Stop timer 
                this._PidVM.TimerGraph.Stop(); 
                
                // Assign `_GraphCanvasVM` as `gcvm` for convinience 
                GraphCanvasVM gcvm = this._PidVM._GraphCanvasVM; 

                // Say that timer is not enabled 
                gcvm.IsTimerEnabled = false; 

                // Set reference point to be able to change SP while timer isn't enabled
                Point refpoint = new Point(gcvm.SetpointLeft, gcvm.SetpointTop + 2.5); 
                gcvm.SpRefPoint = refpoint; 
            }
            catch (System.Exception e)
            {
                ExceptionViewer.WatchExceptionMessageBox(e);
            }
        }
    }
}