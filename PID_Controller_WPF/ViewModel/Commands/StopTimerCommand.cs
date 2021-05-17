using System.Windows; 
using System.Windows.Input; 
using PID_Controller_WPF.ViewModel; 
using PID_Controller_WPF.Exceptions; 

namespace PID_Controller_WPF.Commands
{
    class StopTimerCommand : ICommand
    {
        #region Members
        private PidViewModel _PidViewModel; 
        #endregion  // Members

        #region Constructor
        public StopTimerCommand(PidViewModel pidViewModel)
        {
            this._PidViewModel = pidViewModel; 
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
                this._PidViewModel.TimerGraph.Stop(); 
                
                // Assign `_GraphCanvasViewModel` as `gcvm` for convinience 
                GraphCanvasViewModel gcvm = this._PidViewModel._GraphCanvasViewModel; 

                // Say that timer is not enabled 
                gcvm.IsTimerEnabled = false; 

                // Set reference point to be able to change SP while timer isn't enabled
                Point refpoint = new Point(gcvm.SetpointLeft, gcvm.SetpointTop + 2.5); 
                gcvm.ReferencePoint = refpoint; 
            }
            catch (System.Exception e)
            {
                ExceptionViewer.WatchExceptionMessageBox(e);
            }
        }
    }
}