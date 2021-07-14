using System; 
using System.Windows.Input;
using PidControllerWpf.ViewModel; 

namespace PidControllerWpf.Commands
{
    public class RegulatePvCommand : ICommand
    {
        public PidViewModel _PidViewModel { get; set; }

        public RegulatePvCommand(PidViewModel pidViewModel)
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
            double pvDelta = 1;      // Delta for process variable
            string direction = parameter as string;
            if (direction == "Decrease")
            {
                pvDelta = -pvDelta; 
            }
            this._PidViewModel.ChangeProcessVariable(pvDelta);
        }
    }
}