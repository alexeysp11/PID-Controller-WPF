using System; 
using System.Windows.Input;
using PID_Controller_WPF.ViewModel; 

namespace PID_Controller_WPF.Commands
{
    public class SetpointDownCommand : ICommand
    {
        public PidViewModel _PidViewModel { get; set; }

        public SetpointDownCommand(PidViewModel pidViewModel)
        {
            this._PidViewModel = pidViewModel; 
        }

        public event EventHandler CanExecuteChanged; 
        
        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            this._PidViewModel.ChangeSetpoint(-1.0f);
        }
    }
}