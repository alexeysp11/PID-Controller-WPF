using System; 
using System.Windows.Input;
using PidControllerWpf.ViewModel; 

namespace PidControllerWpf.Commands
{
    public class ChangeSetpointCommand : ICommand
    {
        public PidViewModel _PidViewModel { get; set; }

        public ChangeSetpointCommand(PidViewModel pidViewModel)
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
            double setpointDelta = 1;      // Delta for process variable
            string direction = parameter as string;
            if (direction == "Decrease")
            {
                setpointDelta = -setpointDelta; 
            }
            this._PidViewModel.ChangeSetpoint(setpointDelta);
        }
    }
}