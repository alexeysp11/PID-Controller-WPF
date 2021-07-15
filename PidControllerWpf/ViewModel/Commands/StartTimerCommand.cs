using System.Windows.Input; 
using PidControllerWpf.ViewModel; 
using PidControllerWpf.Exceptions; 

namespace PidControllerWpf.Commands
{
    public class StartTimerCommand : ICommand 
    {
        #region Members
        private PidViewModel _PidViewModel; 
        #endregion  // Members

        #region Constructor
        public StartTimerCommand(PidViewModel pidViewModel)
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
                this._PidViewModel.TimerGraph.Start(); 
                this._PidViewModel._GraphCanvasVM.IsTimerEnabled = true; 
            }
            catch (System.Exception e)
            {
                ExceptionViewer.WatchExceptionMessageBox(e);
            }
        }
    }
}