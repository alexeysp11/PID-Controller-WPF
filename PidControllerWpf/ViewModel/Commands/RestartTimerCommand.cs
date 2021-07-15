using System.Windows; 
using System.Windows.Input; 
using PidControllerWpf.View; 
using PidControllerWpf.ViewModel; 
using PidControllerWpf.Exceptions; 

namespace PidControllerWpf.Commands
{
    class RestartTimerCommand : ICommand
    {
        #region Members
        private PidViewModel _PidViewModel; 
        #endregion  // Members

        #region Constructor
        public RestartTimerCommand(PidViewModel pidViewModel)
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
                
                // Assign `_GraphCanvasVM` as `gcvm` for convinience 
                GraphCanvasVM gcvm = this._PidViewModel._GraphCanvasVM; 

                // Say that timer is not enabled 
                gcvm.IsTimerEnabled = false; 

                // Set size of a window
                MainWindow.MinTimeGraph = 0; 
                MainWindow.MaxTimeGraph = 60; 

                // Set SP to zero 
                gcvm.Setpoint = 0; 
                _PidViewModel._TextBlockViewModel.SetPointTextBlock = $"{gcvm.Setpoint}"; 
                
                // Set PV to zero 
                gcvm.ProcessVariable = 0; 
                _PidViewModel._TextBlockViewModel.ProcessVariableTextBlock = $"{gcvm.ProcessVariable}"; 

                // Set time to zero
                gcvm.Time = 0; 
                _PidViewModel._TextBlockViewModel.TimeTextBlock = $"{gcvm.Time}";

                // Set reference point to be able to change SP while timer isn't enabled
                Point refpoint = new Point(gcvm.SetpointLeft, gcvm.SetpointTop + 2.5); 
                gcvm.SpRefPoint = refpoint; 

                // Clear list of lines 
                gcvm.ClearListOfLines(); 
            }
            catch (System.Exception e)
            {
                ExceptionViewer.WatchExceptionMessageBox(e);
            }
        }
    }
}