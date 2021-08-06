using System.Windows.Input; 
using PidControllerWpf.ViewModel; 

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
                System.Windows.MessageBox.Show($"Exception: {e}", "Exception");
            }
        }
    }
}