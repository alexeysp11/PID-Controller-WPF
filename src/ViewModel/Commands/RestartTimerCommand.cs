using System.Windows; 
using System.Windows.Input; 
using PidControllerWpf.View; 
using PidControllerWpf.ViewModel; 

namespace PidControllerWpf.Commands
{
    class RestartTimerCommand : ICommand
    {
        #region Members
        private PidVM _PidVM; 
        #endregion  // Members

        #region Constructor
        public RestartTimerCommand(PidVM PidVM)
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

                // Set size of a window
                MainWindow.MinTimeGraph = MainWindow.InitMinTimeGraph; 
                MainWindow.MaxTimeGraph = MainWindow.InitMaxTimeGraph; 

                // Set SP to zero 
                gcvm.Setpoint = 0; 
                _PidVM._TextBlockVM.SetPointTextBlock = $"{gcvm.Setpoint}"; 
                
                // Set PV to zero 
                gcvm.ProcessVariable = 0; 
                _PidVM._TextBlockVM.ProcessVariableTextBlock = $"{gcvm.ProcessVariable}"; 

                // Set time to zero
                gcvm.Time = 0; 
                _PidVM._TextBlockVM.TimeTextBlock = $"{gcvm.Time}";

                // Set reference point to be able to change SP while timer isn't enabled
                Point refpoint = new Point(gcvm.SetpointLeft, gcvm.SetpointTop + 2.5); 
                gcvm.SpRefPoint = refpoint; 

                // Clear list of lines 
                gcvm.ClearListOfLines(); 
            }
            catch (System.Exception e)
            {
                System.Windows.MessageBox.Show($"Exception: {e}", "Exception");
            }
        }
    }
}