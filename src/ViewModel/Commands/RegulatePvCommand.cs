using System; 
using System.Windows.Input;
using PidControllerWpf.ViewModel; 

namespace PidControllerWpf.Commands
{
    public class RegulatePvCommand : ICommand
    {
        public PidVM _PidVM { get; set; }

        public RegulatePvCommand(PidVM PidVM)
        {
            this._PidVM = PidVM; 
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
            this._PidVM.ChangeProcessVariable(pvDelta);
        }
    }
}