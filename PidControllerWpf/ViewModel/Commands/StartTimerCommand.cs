using System.Windows.Input; 
using PidControllerWpf.ViewModel; 
using PidControllerWpf.Exceptions; 

namespace PidControllerWpf.Commands
{
    public class StartTimerCommand : ICommand 
    {
        #region Members
        private PidVM _PidVM; 
        #endregion  // Members

        #region Constructor
        public StartTimerCommand(PidVM PidVM)
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
                this._PidVM.TimerGraph.Start(); 
                this._PidVM._GraphCanvasVM.IsTimerEnabled = true; 
            }
            catch (System.Exception e)
            {
                ExceptionViewer.WatchExceptionMessageBox(e);
            }
        }
    }
}