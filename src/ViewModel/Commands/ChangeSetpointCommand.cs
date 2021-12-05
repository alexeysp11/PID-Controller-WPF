using System; 
using System.Windows.Input;
using PidControllerWpf.ViewModel; 

namespace PidControllerWpf.Commands
{
    public class ChangeSetpointCommand : ICommand
    {
        public PidVM PidVM { get; set; }

        public ChangeSetpointCommand(PidVM pidVM)
        {
            this.PidVM = pidVM; 
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
            this.PidVM.ChangeSetpoint(setpointDelta);
        }
    }
}